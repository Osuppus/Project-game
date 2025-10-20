using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    public class JumpMechanic : MonoBehaviour
    {
        public float jumpForce = 7f;
        Rigidbody2D rb;
        int jumpCounter;            // Counts how many times the player has jumped.
        public int maxJumps = 1;           // The number of times the player can jump before touching the ground again.
        Vector2 position;           // To store the position of the player.
        Vector2 direction = Vector2.down; // Part of the ground-check raycast maneuver below.
        public float distance;  // This is adjustable to accomodate future changes to the player sprite.
        private LayerMask groundLayer;
        private RaycastHit2D groundSensor; // A ray fired from 'position' straight down at a 'distance' of just below the player's feet.
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();  // This object's RigidBody2D component, for short.
            groundLayer = LayerMask.GetMask("Ground");
        }

        void Update()
        {
            position = transform.position;      // Keep 'position' updated with the player's present position.
            
            // Jump input
            if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < maxJumps)  // If the player presses 'Space' AND the current jump counter does not exceed the maximum...
            {
                jumpCounter++;  // Increment the counter by 1 each time the player jumps, then
                print("jumpCounter = " +  jumpCounter);
                rb.linearVelocityY = 0; // reset vertical velocity so each jump is equally strong, and finally
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // add impulse force to the character's positive Y axis, according to the current jump force.
            }
            
            
            // / Ground Check Logic:
            // - The RaycastHit2D variable named 'groundSensor' will now be supplied the hit data from a 2D ray.
            //      - groundSensor was declared at the above with the other variables, but not defined. It will now be defined, below:
            // - The ray will originate at the player's 'position'. (transform.position)
            // - The 'direction' will be straight down. (Vector2.down)
            // - The 'distance' the ray will travel will terminate it at a point just below the player's feet. (adjustible in the Unity Editor)
            // - The ray will exist only on the "Ground" layer. Thereby, it can only "hit" objects that are also on that layer.
            groundSensor = Physics2D.Raycast(position, direction, distance, groundLayer);
            
            if (groundSensor)  // the 'groundSensor' (a RaycastHit2D) will return "true" if it contains any information at all, and "null" if it doesn't.
            {
                print("There's the ground!");
                jumpCounter = 0;  // Restore the player's ability to jump, because their feet have touched the ground.
            }
            Debug.DrawLine(position, new Vector2(position.x, position.y - distance), Color.red);  //Visualize the sensor ray in the editor for calibration.
            
        }
    }
}