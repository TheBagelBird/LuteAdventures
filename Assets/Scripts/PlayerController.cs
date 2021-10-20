using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Speed of the player
    [SerializeField]
    private float moveSpeed = 5;

    //The move point, basically where the player will go, it's also used to check for obstacles in the "Stop Movement" Layer
    [SerializeField]
    private Transform movePoint;

    //Layer with obstacles named "Stop Movement" that stops the player from moving
    [SerializeField]
    private LayerMask obstacleMask;

    //variable to retrieve the action assets
    InputActionAsset actions;

    //Almost exactly like Start(), just starts faster
    private void Awake()
    {
        //remove the move point from the player cause I only did that for the sake of organization
        movePoint.parent = null;
        // Retrieve the action asset
        actions = GetComponent<PlayerInput>().actions;
    }


    //Does code every frame basically
    void Update()
    {
        //Makes it so that the player follows the move point every update
        transform.position = Vector2.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // Retrieve the current movement input (horizontal, vertical)
        // You can save this action in a variable in the Awake too if you plan to use it multiple times (like during each Update)
        Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();

        //Makes it so that you can only move if the distance between the move point and the player is below or equal 0.5 units
        if (Vector2.Distance(transform.position, movePoint.position) <= .5f)
        {
            // Check if we have horizontal movement
            if (movement.x != 0f)
            {
                //Checks if there is any obstacle in the way in the "Stop Movement" layer and keeps the player from moving if there is any
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(movement.x, 0f, 0f), .2f, obstacleMask))
                {
                    movePoint.position += new Vector3(movement.x, 0f, 0f);
                }
            }
            else
            // Check if we have vertical movement
            if (movement.y != 0f)
            {
                //Checks if there is any obstacle in the way in the "Stop Movement" layer and keeps the player from moving if there is any
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, movement.y, 0f), .2f, obstacleMask))
                {
                    movePoint.position += new Vector3(0f, movement.y, 0f);
                }
            }
        }
    }
}

