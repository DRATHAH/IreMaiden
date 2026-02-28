using System.Collections;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    //Player GameObject Definition
    private GameObject player;

    //Nav mesh agent
    private UnityEngine.AI.NavMeshAgent enemyNav;

    //Movement Vars
        //Enemy Movement Speed, pulled from nav mesh agent
    private float moveSpeed;
        //Desired velocity used in rotation function, repurposed from an old movement function
    private Vector3 desiredVelocity;
        //Enemy's rigidbody
    private Rigidbody rb;
        //Enemy's acceleration, pulled from nav mesh agent
    private float acceleration;

    //Rotation Vars
        //How fast the enemy can rotate to face player while not moving
    public float rotationSpeed;

    //Stopping Vars
        //How close the enemy needs to be to the player prior to stopping
    public float stoppingDistance = 4;
        //Stopping bools used for communicating with KnightAttack
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public bool stop = false;

    void Start()
    {
        //Define the enemy's nav mesh agent component
        enemyNav = this.GetComponent<UnityEngine.AI.NavMeshAgent>();

        //Define the moveSpeed component based on the agent
        moveSpeed = enemyNav.speed;

        //Define acceleration based on nav mesh agent's
        acceleration = enemyNav.acceleration;

        //Define the rigidbody for rotation purposes
        rb = this.GetComponent<Rigidbody>();
    }

    /*
     *  Determining the player is done on enable just in case the player prefab is destroyed for whatever reason
     *  This can be moved to start if player death is handled without doing that, this is just in case.
     */
    void OnEnable()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //Determine where the nav agent should move
        enemyNav.SetDestination(player.transform.position);

        //Stop the nav agent from moving if they are attacking
        if (attacking == true)
        {
            enemyNav.isStopped = true;
        }
        else
        {
            enemyNav.isStopped = false;
        }

        /*
         * Set the desired velocity, used in rotation calc
         * Probably could be optimized in some way but I am unsure.
         */
        desiredVelocity = new Vector3(enemyNav.destination.x - transform.position.x, 0f, enemyNav.destination.z - transform.position.z) * moveSpeed;
    }

    //Stuff related to enemy movement, if this can be optimized please do so
    void FixedUpdate()
    {
        //Rotate the enemy to face player if within a certain range
        if (Vector3.Distance(player.transform.position, transform.position) < stoppingDistance && stop == false && attacking == false)
        {
            enemyNav.speed = 0;
            Rotation();
        }
        //Stop the enemy if they're within a certain range or attacking
        else if (stop == true || attacking == true)
        {
            if (enemyNav.speed > 0)
            {
                enemyNav.speed = 0;
            }
        }
        //Have the enemy start moving towards their full speed if the player is far enough away
        else if (stop == false || attacking == false)
        {
            if (enemyNav.speed < moveSpeed)
            {
                enemyNav.speed += Time.deltaTime * acceleration;
            }
        }
        //Prevent enemy speed from going above their maximum speed, or 0 to not break things.
        enemyNav.speed = Mathf.Clamp(enemyNav.speed, 0, moveSpeed);
    }


    /* 
     * Function for rotating the enemy towards the player if they're within a certain distance
     * This is done this way to prevent the nav mesh agent from ramming into the player, causing problems
     */
    private void Rotation()
    {
        if(desiredVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime));
        }
    }
}
