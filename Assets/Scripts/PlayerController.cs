using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private Transform movePoint;
    [SerializeField]
    private LayerMask obstacleMask;

    InputActionAsset actions;


    private void Awake()
    {
        //remove the move point from the player cause I only did that for the sake of organization
        movePoint.parent = null;
        // Retrieve the action asset
        actions = GetComponent<PlayerInput>().actions;

    }


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
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(movement.x, 0f, 0f), .2f, obstacleMask))
                {
                    movePoint.position += new Vector3(movement.x, 0f, 0f);
                }
            } else
            // Check if we have vertical movement
            if (movement.y != 0f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, movement.y, 0f), .2f, obstacleMask))
                {
                    movePoint.position += new Vector3(0f, movement.y, 0f);
                }
            }
        }
    }
}

