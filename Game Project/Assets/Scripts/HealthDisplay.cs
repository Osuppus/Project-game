using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mechanics
{
    public class HealthDisplay : MonoBehaviour
    {
        // Variables
        GameObject[] heartsArray;
        private int maxHealth;
        private GameObject heartsContainer;
        
        void Start()
        {
            maxHealth = HealthMechanic.maxHealth;
            heartsArray = new GameObject[maxHealth];
            heartsContainer = GameObject.Find("displayHeartsContainer");

            foreach (GameObject heart in heartsContainer)
            {
                
            }

        }

        // Update is called once per frame
        void Update()
        {
            // displayed hearts is the same as currentHealth.
            // For each display-heart: 
            //      if display-heart(number) is greater than currentHealth:
            //          set active to false,
            //      else: set active to true?

        }
    }
}
