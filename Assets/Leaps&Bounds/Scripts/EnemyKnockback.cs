using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{

    private Rigidbody2D rb;
    public int thrust;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(GameObject enemyPlayer)
    {
        float xDist = transform.position.x - enemyPlayer.transform.position.x;
        int forceFromLeftOrRight = xDist > 0 ? 1 : -1;
        float knockForce = thrust * (1 / (Mathf.Abs(0.1f * xDist) + 1));
        rb.AddForce(transform.right * forceFromLeftOrRight * knockForce);
        rb.AddForce(transform.up * knockForce);
    }
}
