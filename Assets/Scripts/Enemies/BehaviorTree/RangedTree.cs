using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using IM;

public class RangedTree : MonoBehaviour
{
    BehaviorTree tree;
    public Transform player;

    public enum ActionState
    {
        IDLE,
        WORKING
    }
    ActionState state = ActionState.IDLE;
    NavMeshAgent agent;
    Node.Status treeStatus = Node.Status.RUNNING;

    [Header("Stats")]
    public float sightRange = 100;
    public float attackRange = 100;
    public float retreatRange = 10f;
    public float attackSpeed = 1;
    public int damage = 5;
    public float arrowSpeed = 100f;
    public GameObject projectile;
    public Transform arrowSpawn;

    float timeBetweenAttack = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviorTree();
        Sequence idleLoop = new Sequence("Idle or Act");
        Leaf detectPlayer = new Leaf("Player In Sight", PlayerInSight);
        Selector rangeIncrement = new Selector("Get Player In Range");
        Leaf playerTooClose = new Leaf("Player Too Close", Retreat);
        Leaf playerTooFar = new Leaf("Player Too Far", Approach);
        Sequence attackSequence = new Sequence("Attack the Player");
        Leaf shoot = new Leaf("Shoot the Player", RangedAttack);
        //Leaf reposition = new Leaf("Reposition After Attack", Reposition);

        idleLoop.AddChild(detectPlayer);
        idleLoop.AddChild(rangeIncrement);
        rangeIncrement.AddChild(playerTooClose);
        rangeIncrement.AddChild(playerTooFar);
        rangeIncrement.AddChild(attackSequence);
        attackSequence.AddChild(shoot);
        //attackSequence.AddChild(reposition);
        tree.AddChild(idleLoop);
        tree.AddChild(rangeIncrement);
        
        tree.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
        {
            treeStatus = tree.Process();
        }

        timeBetweenAttack += Time.deltaTime;
    }

    public Node.Status PlayerInSight()
    {
        if (GameManager.instance.PlayerContainer != null)
        {
            player = GameManager.instance.PlayerContainer.GetComponentInChildren<PlayerHealth>().transform;
            float distance = (transform.position - player.position).magnitude;
            if (distance < sightRange)
            {
                return Node.Status.SUCCESS;
            }
        }

        return Node.Status.FAILURE;
    }

    public Node.Status Retreat()
    {
        if ((transform.position - player.position).magnitude < retreatRange)
        {
            return GoToLocation(transform.position + (transform.forward * -retreatRange));
        }
        return Node.Status.FAILURE;
    }

    public Node.Status Approach()
    {
        if ((transform.position - player.position).magnitude > attackRange)
        {
            Debug.Log("approach");
            return GoToLocation(player.position);
        }
        else if ((transform.position - player.position).magnitude <= attackRange)
        {
            agent.SetDestination(transform.position);
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status RangedAttack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.position);
        if (timeBetweenAttack >= attackSpeed)
        {
            Debug.Log("attack");
            GameObject arrow = Instantiate(projectile, arrowSpawn.position + (transform.forward * 0.5f), Quaternion.identity);
            Projectile proj = arrow.GetComponent<Projectile>();
            proj.Initialize(false, damage, arrowSpeed, (player.position - arrowSpawn.position).normalized, false, 0, "Enemy");
            timeBetweenAttack = 0;
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    /*public Node.Status Reposition()
    {

    }*/

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(transform.position, destination);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, retreatRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
