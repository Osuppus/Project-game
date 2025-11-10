using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = System.Drawing.Color;
using UnityEngine.Events;

namespace Mechanics
{
    public class HealthMechanic : MonoBehaviour
    {
        // Variables Declaration Zone
        SpriteRenderer spriteRenderer;
        public float maxHealth;
        float currentHealth;
        bool gotHit = false;
        bool gotHealed = false;
        public LayerMask ouchies;  // In the editor this is set to include the layers "Enemy" and "Hazard"
        public LayerMask yummies;  // This currently doesn't contain anything because we don't have any healing objects or associated layers.
        float dmgMod;


        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            currentHealth = maxHealth;  // The player starts at full health.
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Health: " +  currentHealth + "/" +  maxHealth); //  Display health status, in absence of UI feedback.
            if (yummies.Contains(collision.gameObject.layer))
            {
                gotHealed = true;  // Flip this so it gets processed on the next FixedUpdate call.
                dmgMod = collision.gameObject.GetComponent<HarmDoer>().harmMod;  // Examine the HarmDoer script attached to the
                                                                                 // colliding object, and transfer the value of the 'harmMod' var found there
                                                                                 // into the 'dmgMod' var found here.

            }
            
            if (ouchies.Contains(collision.gameObject.layer))
            {
                gotHit = true;  // Same as above.
                dmgMod = collision.gameObject.GetComponent<HarmDoer>().harmMod;  // Same as above.
            }
        }
        void FixedUpdate()
        {
            // Healing damage appears first in the script to ensure that if the player is healed at the same instant
            // that they take what would have been lethal damage - the heal occurs first, potentially preventing death.
            if (gotHealed)
            {
                HealDamage(dmgMod);
            }
            
            // Take damage
            if (gotHit)
            {
                TakeDamage(dmgMod);
            }

            
            
        }

        // Restore health
        private void HealDamage(float modifier = 1f)
        {
            // Safety catch: In the unlikely event that the value is null, restore it to its default value.
            if (modifier == null)
            {
                modifier = 1;
            }
            
            // Tick up the player's health by 1 multiplied by the modifier plucked from the colliding object's HarmDoer script.
            currentHealth += 1 * modifier;
            
            // Prevent overhealing
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            gotHealed = false;
        }


        void TakeDamage(float modifier = 1)
        {
            // Same failsafe as above (... HealDamage() ...)
            if (modifier == null)
            {
                modifier = 1;
            }
            
            // Reduce current health by the amount given in the call, with a default of 1 if no amount is specified.
            currentHealth -= 1 * modifier;
            
            // Prevent negative health. Zero should be the minimum. This is just to prevent unwanted unexpected behavior later on.
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            
            // People die if they are killed
            if (currentHealth <= 0)
            {
                // The Bart, The
                Die();
            }
            
            gotHit = false;  //  Reset the switch so it can be tripped again later.
        }
        
        void Die()
        {
            // Death behavior. Currently, all this does is turn the player red. This is placeholder behavior.
            spriteRenderer.color = UnityEngine.Color.red;
        }
    }
    
    public static class UnityExtensions
    {
        /// <summary>
        /// Extension method to check if a layer is in a layermask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
