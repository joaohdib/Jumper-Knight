using UnityEngine;

public class Slime : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private bool movingRight = true;

    [Header("Detection")]
    public Transform wallCheck;
    public Transform headCheck;
    public float detectionRadius = 0.2f;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private Rigidbody2D rb;

    private bool isActivated;

    void OnBecameVisible()
    {
        isActivated = true;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check for walls in front of the slime
        bool hitWall = Physics2D.OverlapCircle(wallCheck.position, detectionRadius, wallLayer);

        Collider2D playerCollider = Physics2D.OverlapCircle(headCheck.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {
            // get the player's Rigidbody to make them bounce
            Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // reset Y velocity and add a bounce force
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 7f);
            }

            Die();
        }

        if (hitWall)
        {
            Flip();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (isActivated)
        {
            float horizontalVelocity = movingRight ? moveSpeed : -moveSpeed;
            rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

    }

    private void Flip()
    {
        movingRight = !movingRight;

        transform.Rotate(0f, 180f, 0f);  // rotate the enemy object 180 degrees to face the other side
    }

    private void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, detectionRadius);
        }

        if (headCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(headCheck.position, detectionRadius);
        }
    }

}
