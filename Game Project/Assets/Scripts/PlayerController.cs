using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isFacingRight = true;
    private float horizontal;
    public float speedMultiplyer = 1f;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] Rigidbody2D rb;


    void Start()
    {
        SpeedItem.OnSpeedCollected += StartSpeedBoost; //What the speed item does when interacted with
    }

    void StartSpeedBoost(float multiplyer)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplyer)); //initiates speed boost
    }
    
    IEnumerator SpeedBoostCoroutine(float multiplyer)
    {
        speedMultiplyer = multiplyer;
        yield return new WaitForSeconds(5f); //The amount of time the speed up abililty lasts
        speedMultiplyer = 1f; // After the time limit, sets speed multiplyer back to normal
    }
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (Input.GetKey("d"))
        {
            newPosition.x += moveSpeed * speedMultiplyer * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            newPosition.x -= moveSpeed * speedMultiplyer * Time.deltaTime;
        }

        transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (isDashing)
        {
            return;
        }

        Flip();
    }
    
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) // To make player dash in more than one direction
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    

    private IEnumerator Dash()
    {
        canDash = false; // Prevent further dashing until cooldown is over
        isDashing = true; // Indicate that a dash is in progress
        float originalGravity = rb.gravityScale; // Store current gravity so we can restore it after the dash
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
