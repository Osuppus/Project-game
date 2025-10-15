using UnityEngine;

namespace Mechanics
{
    public class JumpMechanic : MonoBehaviour
    {
        public float jumpForce = 7f; // You can change this in the Inspector
        Rigidbody2D rb;
        bool isGrounded;
        bool canDoubleJump;

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
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    canDoubleJump = true; // Allow double jump after the first jump
                }
                else if (canDoubleJump)
                {
                    // Double jump
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    canDoubleJump = false; // Consume double jump
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if player landed on ground
            if (collision.gameObject.CompareTag("Ground"))      //This is giving True when touching any part of platform, including the bottom.
            {
                isGrounded = true;
                canDoubleJump = false;
            }
        }

        void OnCollisionExit2D(Collision2D collision)       //Not sure if this is how we wanna do this. Might be causing problems. -E
        {
            // Check if player left the ground
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }


    }
}