using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

   
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (Input.GetKey("d"))
        {
            newPosition.x += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            newPosition.x -= moveSpeed * Time.deltaTime;
        }

        transform.position = newPosition;
    }
    
   
}
