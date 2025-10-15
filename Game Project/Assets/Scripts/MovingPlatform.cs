using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
   
    public Transform pointA;
    public Transform pointB;
    public float speed = 1.5f;

    private Vector3 nextPosition;


    void Start()
    {
        nextPosition = pointB.position; //  Setting the initial target position to pointB
    }

    void Update()  
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime); //  the object moves towards the nextPosition at the given speed
        
        if(transform.position == nextPosition)     // If the object has reached the nextPosition, switch to the other point
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position; // switch between pointA and pointB
        }
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // // This allows the player to move along with the platform by making the player the child object
        {
            collision.transform.SetParent(transform); // Replace this with something less binding, like a Position Constraint component. (Check Brackeys tutorial) -E
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null); // Removes the player from being a child object when they hop off the platform
    }
    
}
