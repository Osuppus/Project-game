using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 1.8f;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        //Let PlayerController script know: Once Player made contact, 
        // intiate speed item ability and delete game object of speed item
        OnSpeedCollected.Invoke(speedMultiplier);
        Destroy(gameObject);
    }
    
}
