using UnityEngine;

public class JumpMechanic : MonoBehaviour
{
    public float jumpForce = 7f; // You can change this in the Inspector
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private bool canDoubleJump;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // Regular jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = true; // Allow double jump after the first jump
            }
            else if (canDoubleJump)
            {
                // Double jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false; // Consume double jump
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player landed on ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    
}
