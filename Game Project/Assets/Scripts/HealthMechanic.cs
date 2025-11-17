using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = System.Drawing.Color;
using UnityEngine.Events;

namespace Mechanics
{
    public class HealthMechanic : MonoBehaviour
    {
        // Variables
        SpriteRenderer spriteRenderer;
        public static int maxHealth;    // The player's maximum health. Adjustable in the Unity Editor.
        float currentHealth;            // The player's current health.
        bool gotHit = false;            // A trigger to activate the process for taking damage.
        bool gotHealed = false;         // A trigger to activate the process for restoring health.
        public LayerMask ouchies;       // In the editor this is set to include the layers "Enemy" and "Hazard"
        public LayerMask yummies;       // This currently doesn't contain anything because we don't have any healing objects or associated layers.
        float dmgMod;                   // 'damage Modifier': An optional modifier with which to adjust individual damage and healing amounts. Acts as a multiplier.


        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();  // Assign the spriterenderer to the variable of the same name, for ease of use/readability.
            currentHealth = maxHealth;  // The player starts at full health.
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Health: " +  currentHealth + "/" +  maxHealth); //  Display health status, in absence of UI feedback.
            
            // If the player collides with a healing object or effect, the player's health is restored.
            // If the player collides with a harmful object or effect, the player's health is reduced.
            // These happen in that order (heal first, then take damage), so that if case the player takes damage and is healed at the same time - the healing is processed first, possibly preventing death.
            
            // --GET HEALED--
            // More specifically: If [the colliding object] is on a layer which appears in the layermask ("yummies");  Activate the healing function.
            if (yummies.Contains(collision.gameObject.layer))
            {
                gotHealed = true;  // Make this <true> so HealDamage() is called on the next FixedUpdate call.
                dmgMod = collision.gameObject.GetComponent<HarmDoer>().harmMod;  // Examine the HarmDoer script attached to the
                                                                                 // colliding object, and transfer the value of the 'harmMod' variable from there
                                                                                 // into the 'dmgMod' variable in here.

            }
            
            // --GET HURT--
            // More specifically: If [the colliding object] is on a layer which appears in the layermask ("ouchies");  Activate the damaging function.
            if (ouchies.Contains(collision.gameObject.layer))
            {
                gotHit = true;  // Same as above, but for gotHit instead of gotHealed.
                dmgMod = collision.gameObject.GetComponent<HarmDoer>().harmMod;  // Exactly the same as above.
            }
        }
        void FixedUpdate()
        {
            // Healing damage (aka "restoring health") appears first in the script to ensure that if the player is healed at the same instant
            // they would take lethal damage - the heal occurs first, potentially preventing death.
            
            // If the player got healed, call the [Heal Damage] function.
            if (gotHealed)
            {
                HealDamage(dmgMod);
            }
            
            // If the player got hit, call the [Take Damage] function.
            if (gotHit)
            {
                TakeDamage(dmgMod);
            }

            
            
        }

        // The Heal Damage function
        private void HealDamage(float modifier = 1f)    // The HealDamage and TakeDamage functions can be given a
                                                        // modifier to adjust how much health they restore or destroy.
                                                        // If not given a modifier, they will default to a value of 1 (1*1=1).
                                                        // The modifier is a float so that you can reduce the end value by multiplying by less than 1.
        {
            // Tick up the player's health by 1 (multiplied by the modifier plucked from the colliding object's HarmDoer script, if it exists).
            currentHealth += 1 * modifier;
            
            // Prevent overhealing: If the current health is now greater than the maximum health, set current health to the same value as max health.
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            gotHealed = false;  // Set this back to false when it's finished, so it's ready to trigger again.
        }

        // The Take Damage function
        void TakeDamage(float modifier = 1)             // See above (HealDamage) re: modifier and default value.
        {
            // Reduce current health by the amount given in the call, with a default of 1 if no amount is specified. (Same as above, but reducing health instead of restoring).
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
