using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    //variable for the player's position
    [SerializeField]
    public GameObject playerSprite;

    [SerializeField] private int healthPlayer = 6;
    [SerializeField] private Text healthPlayerText;

    [SerializeField]
    private bool attackButtonPressed;


    //variables for checking the input for attacks
    private float upAttack;
    private float downAttack;
    private float leftAttack;
    private float rightAttack;

    private void OnCollisionEnter2D(Collision2D other)
    {


        Debug.Log("Player has collided with something");

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("supposed to destroy the enemy ");
            //Destroy(other.gameObject);


            healthPlayer -= 1;
            healthPlayerText.text = "healthPlayer  : " + healthPlayer.ToString();
        }



    }

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


        //variables for checking the value of the actions
        float upAttack = actions.FindAction("Attack Up").ReadValue<float>();
        float downAttack = actions.FindAction("Attack Down").ReadValue<float>();
        float leftAttack = actions.FindAction("Attack Left").ReadValue<float>();
        float rightAttack = actions.FindAction("Attack Right").ReadValue<float>();




        //Makes it so that you can only move if the distance between the move point and the player is below or equal 0.5 units
        //The Player Also cannot move if the modifiers that are used to attack are pressed, meaning they cannot attack and move at the same time
        if (Vector2.Distance(transform.position, movePoint.position) <= 0.4f && upAttack.Equals(0) && downAttack.Equals(0) && rightAttack.Equals(0) && leftAttack.Equals(0))
        {
            // Check if we have horizontal movement
            if (movement.x != 0f)
            {

                /*
               Debug.Log("*************************" );
               Debug.Log("movement sideways " );
               Debug.Log("movement.y : " + movement.y);
               Debug.Log("movement.x : " + movement.x);
               */
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
                /*
                Debug.Log("*************************");
                Debug.Log("movement vertical ");
                Debug.Log("movement.y : " + movement.y);
                Debug.Log("movement.x : " + movement.x);
                */

                //Checks if there is any obstacle in the way in the "Stop Movement" layer and keeps the player from moving if there is any
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, movement.y, 0f), .2f, obstacleMask))
                {
                    movePoint.position += new Vector3(0f, movement.y, 0f);
                }
            }
        }


        //The attacks
        if (upAttack != 0)
        {
            if (Physics2D.OverlapBox(playerSprite.transform.position + new Vector3(0, 1, 0), new Vector2(1, 1), 0))
            {
                Debug.Log("You killed the enemy using an up attack");
            }
        }


        if (downAttack != 0)
        {
            if (Physics2D.OverlapBox(playerSprite.transform.position + new Vector3(0, -1, 0), new Vector2(1, 1), 0))
            {
                Debug.Log("You killed the enemy using an down attack");
            }
        }

        if (leftAttack != 0)
        {
            if (Physics2D.OverlapBox(playerSprite.transform.position + new Vector3(-1, 0, 0), new Vector2(1, 1), 0))
            {
                Debug.Log("You killed the enemy using an left attack");
            }
        }

        if (rightAttack != 0)
        {
            if (Physics2D.OverlapBox(playerSprite.transform.position + new Vector3(1, 0, 0), new Vector2(1, 1), 0))
            {
                Debug.Log("You killed the enemy using a right attack");
            }
        }
    }
}

