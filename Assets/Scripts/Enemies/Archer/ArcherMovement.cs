using UnityEngine;

public class ArcherMovement : MonoBehaviour
{
    //Player GameObject Definition
    private GameObject player;

    //How far away is the player variable?
    private float distanceToPlayer;

    //Nav mesh agent
    private UnityEngine.AI.NavMeshAgent enemyNav;

    //Nav mesh agent move speed
    private float moveSpeed;

    //Nav mesh agent acceleration
    public float acceleration;

    //Rigidbody for turning (Could be replaced with transform.rotation stuff but whatever)
    private Rigidbody rb;

    //Turning direction calculation
    private Vector3 desiredVelocity;

    //Head transform
    public Transform head;

    //Part where the bullets come out transform
    public Transform aimingNode;

    //How fast the dude can turn
    public float rotationSpeed;

    //Attacking script
    private ArcherAttack archerAttack;

    //Is the enemy attacking?
    [HideInInspector] public bool attacking = false;


    void Start()
    {
        

    }

    void OnEnable()
    {
        player = GameObject.Find("Player");

        //Define nav agent
        enemyNav = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //define moveSpeed for stopping and starting
        moveSpeed = enemyNav.speed;

        //attacking script definition
        archerAttack = this.GetComponent<ArcherAttack>();

        //rigidbody definition
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set the end destination for where the enemy should go
        enemyNav.SetDestination(player.transform.position);
        //Calculate the distance the enemy is from the player for the purposes of the complicated raycasting s**t
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Rotation stuff copied from knight code
        desiredVelocity = new Vector3(enemyNav.destination.x - transform.position.x, 0f, enemyNav.destination.z - transform.position.z) * moveSpeed;
    }

    /*This handles enemy movement/targetting for the player.
        THIS NEEDS TO HAVE A FUNCTION FOR BACKING UP FROM THE PLAYER
            That function would look something like this:
                //if(distanceToPlayer < minimum proximity){
                        find random location that can still see the player but is farther away from the player
                   }
                    else{
                        shoot the player
                    }
     */
    void FixedUpdate()
    {
        if(distanceToPlayer < 100)
        {
            //Does the enemy have a potential line of sight with the player?
            if(Physics.Linecast(head.position, player.transform.position, out RaycastHit hit) && hit.collider.name == "Player")
            {
                //If yes, is the enemy facing the player?
                    //Calculate the direction the ray should point in
                Vector3 target = new Vector3((aimingNode.position.x - transform.position.x) * (distanceToPlayer + 10), player.transform.position.y - head.transform.position.y, (aimingNode.position.z - transform.position.z) * (distanceToPlayer + 10));
                    //Cast the ray and check if it is hitting a player
                if (Physics.Raycast(head.position, target, out RaycastHit strike, distanceToPlayer + 10) && strike.collider.name == "Player")
                {
                    //If yes, stop moving and shoot the player
                    enemyNav.speed = 0;
                    enemyNav.isStopped = true;
                    archerAttack.Attack();
                }
                else
                {
                    //If no, stop moving and rotate to face the player
                    enemyNav.speed = 0;
                    enemyNav.isStopped = true;
                    if(attacking == false)
                    {
                        Rotation();
                    }
                }
            }
            //If the enemy doesn't have a line of sight with the player from any direction, start moving towards the player
            else
            {
                SpeedUp();
                enemyNav.isStopped = false;
            }
        }
    }

    //Function for slowing down to a stop (unused currently)
    void SlowDown()
    {
        if(enemyNav.speed > 0)
        {
            enemyNav.speed -= Time.deltaTime * acceleration;
        }
    }

    //Function for getting back to full speed after stopping
    void SpeedUp()
    {
        enemyNav.speed += Time.deltaTime * acceleration;
        Mathf.Clamp(enemyNav.speed, 0, moveSpeed);
    }

    //Function for rotating towards the player while stopped.
    void Rotation()
    {
        if (desiredVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime));
        }
    }
}
