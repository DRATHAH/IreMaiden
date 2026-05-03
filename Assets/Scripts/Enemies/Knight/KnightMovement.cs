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

    private KnightAttack knightAttack;

    //How far away is the player variable?
    private float distanceToPlayer;

    //Head transform
    public Transform head;
    public Transform attackBox;

    public Animator anim;

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

        knightAttack = this.GetComponent<KnightAttack>();

        anim = this.GetComponentInChildren<Animator>();
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
        if(player != null)
        {
            //Determine where the nav agent should move
            enemyNav.SetDestination(player.transform.position);

            //Calculate the distance the enemy is from the player for the purposes of the complicated raycasting s**t
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            //Set rotation stuff
            desiredVelocity = new Vector3(enemyNav.destination.x - transform.position.x, 0f, enemyNav.destination.z - transform.position.z) * moveSpeed;

        }

        //Stop the nav agent from moving if they are attacking
        if (attacking == true)
        {
            enemyNav.isStopped = true;
        }
        else
        {
            enemyNav.isStopped = false;
        }
    }

    //Stuff related to enemy movement, if this can be optimized please do so
    void FixedUpdate()
    {
        //Prevent enemy speed from going above their maximum speed, or 0 to not break things.
        enemyNav.speed = Mathf.Clamp(enemyNav.speed, 0, moveSpeed);
        if(player != null && distanceToPlayer < stoppingDistance && attacking != true)
        {
            //Does the enemy have a potential line of sight with the player?
            if (Physics.Linecast(head.position, player.transform.position, out RaycastHit hit) && hit.collider.name == "Player")
            {
                //If yes, is the enemy facing the player?
                //Calculate the direction the ray should point in
                Vector3 target = new Vector3((attackBox.position.x - transform.position.x) * (distanceToPlayer + 10), player.transform.position.y - head.transform.position.y, (attackBox.position.z - transform.position.z) * (distanceToPlayer + 10));
                //Cast the ray and check if it is hitting a player
                if (Physics.Raycast(head.position, target, out RaycastHit strike, distanceToPlayer + 10) && strike.collider.name == "Player")
                {
                    //If yes, stop moving and shoot the player
                    enemyNav.speed = 0;
                    SlowDown();
                    enemyNav.isStopped = true;
                    knightAttack.Attack();
                }
                else
                {
                    Debug.DrawRay(head.position, target, Color.yellow, distanceToPlayer + 10);

                    //If no, stop moving and rotate to face the player
                    enemyNav.speed = 0;
                    SlowDown();
                    enemyNav.isStopped = true;
                    if (attacking == false)
                    {
                        Rotation();
                    }
                }
            }
        }
        else if(attacking != true)
        {
            enemyNav.isStopped = false;
            if (enemyNav.speed < moveSpeed)
            {
                enemyNav.speed += Time.deltaTime * acceleration;
            }
        }

        anim.SetFloat("Movement", enemyNav.velocity.magnitude);
    }

    void SlowDown()
    {
        Vector3 velocity = enemyNav.velocity;

        velocity.x = 0;
        velocity.z = 0;

        enemyNav.velocity = velocity;
    }


    /* 
     * Function for rotating the enemy towards the player if they're within a certain distance
     * This is done this way to prevent the nav mesh agent from ramming into the player, causing problems
     */
    void Rotation()
    {
        if(desiredVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime));
        }
    }
}
