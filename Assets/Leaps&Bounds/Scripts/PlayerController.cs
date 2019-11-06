using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int moveSpeed = 100;
    public int jumpForce = 400;
    public int thrust = 7500;

    public GameObject enemyPlayer;

    public LayerMask groundLayers;
    public Transform groundCheck;

    private float horizontalMovement = 0;
    private float movementSmoothing = 0.05f;
    private Vector3 zeroVector = Vector3.zero;
    private Rigidbody2D rb = null;

    private bool isGrounded = true;
    private bool jumpPressed = false;
    private bool knockbackEnemy = false;
    private bool beingKnockedback = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
        
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();
        
        Move();

        CheckIfKnocked();
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2(horizontalMovement * Time.fixedDeltaTime * 10f, rb.velocity.y);

        if (!beingKnockedback)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, movementSmoothing);
            //rb.AddForce(targetVelocity * moveSpeed);
        }

        if (isGrounded && jumpPressed)
        {
            isGrounded = false;
            jumpPressed = false;
            knockbackEnemy = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void CheckIfGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayers);
        foreach (Collider2D coll in colliders)
        {
            isGrounded = coll.gameObject != gameObject ? true : false;
            if (isGrounded)
            {
                beingKnockedback = false;

                if (!knockbackEnemy)
                {
                    knockbackEnemy = true;
                    enemyPlayer.GetComponent<EnemyKnockback>().GetKnockedBack(gameObject);
                }
            }
        }
    }

    public void CheckIfKnocked()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            beingKnockedback = true;

            float xDist = transform.position.x - enemyPlayer.transform.position.x;
            int forceFromLeftOrRight = xDist > 0 ? 1 : -1;
            float knockForce = thrust * (1 / (Mathf.Abs(0.1f * xDist) + 1));

            rb.AddForce(transform.right * forceFromLeftOrRight * knockForce);
            rb.AddForce(transform.up * knockForce);
        }
    }

    /*public void GetKnockedBack()
    {
        float xDist = transform.position.x - enemyPlayer.transform.position.x;
        rb.velocity = new Vector2(1 / xDist * knockbackForce, (1 / xDist) * (jumpForce / 10f)); // Invert direction and add knockback force - upward force is half of jump force
    }

    public void GetKnockedBack(Transform otherPlayerPos)
    {
        float xDist = transform.position.x - otherPlayerPos.position.x;
        rb.AddForce(new Vector2(-xDist * knockbackForce, jumpForce / 2)); // Invert direction and add knockback force - upward force is half of jump force
    }*/
}
