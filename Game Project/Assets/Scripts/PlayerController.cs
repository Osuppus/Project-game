using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    bool isFacingRight = true;
    float horizontal;
    public float speedMultiplier = 1f;
    bool canDash = true;
    bool isDashing = false;
    float dashingPower = 24f;
    float dashingTime = 0.2f;
    float dashingCooldown = 1f;
    float originalGravity;
    [SerializeField] Rigidbody2D rb;


    void Start()
    {
        SpeedItem.OnSpeedCollected += StartSpeedBoost; //What the speed item does when interacted with
    }

    void StartSpeedBoost(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier)); //initiates speed boost
    }
    
    IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(5f); //The amount of time the speed up abililty lasts
        speedMultiplier = 1f; // After the time limit, sets speed multiplier back to normal
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (isDashing)
        {
            return;
        }

        // Flip();
    }
    
    // private void Flip()
    // {
    //     if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) // To make player dash in more than one direction
    //     {
    //         Vector3 localScale = transform.localScale;
    //         isFacingRight = !isFacingRight;
    //         localScale.x *= -1f;
    //         transform.localScale = localScale;
    //     }
    // }
    

    private IEnumerator Dash()
    {
        canDash = false; // Prevent further dashing until cooldown is over
        isDashing = true; // Indicate that a dash is in progress
        originalGravity = rb.gravityScale; // Store current gravity so we can restore it after the dash
        rb.gravityScale = 0f; // Temporarily disable gravity during the dash to ensure smooth horizontal movement
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f); // Set the dash velocity in the direction the character is facing
        print(rb.linearVelocity); // Optional debug output of current velocity (useful during testing)
        yield return new WaitForSeconds(dashingTime); // Wait for the dash duration to finish
        rb.gravityScale = originalGravity; // Restore original gravity after dash
        isDashing = false; // Dash is no longer in progress
        yield return new WaitForSeconds(dashingCooldown); // Wait for the cooldown period before allowing another dash
        canDash = true; // Dash can now be used again

        
    }
    
    
   
}
