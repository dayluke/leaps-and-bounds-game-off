using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int moveSpeed = 100;
    public int jumpForce = 400;

    public LayerMask groundLayers;
    public Transform groundCheck;

    private float horizontalMovement = 0;
    private float movementSmoothing = 0.05f;
    private Vector3 zeroVector = Vector3.zero;
    private Rigidbody2D rb = null;
    private bool isGrounded = true;
    private bool jumpPressed = false;

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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayers);
        foreach (Collider2D coll in colliders)
        {
            if (coll.gameObject != gameObject)
            {
                isGrounded = true;
            }
        }

        Move();
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2(horizontalMovement * Time.fixedDeltaTime * 10f, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, movementSmoothing);

        if (isGrounded && jumpPressed)
        {
            isGrounded = false;
            jumpPressed = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }
}
