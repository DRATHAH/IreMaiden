using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using IM;

public class MeleeTree : DamageableCharacter
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
    public float attackSpeed = 1;
    public int damage = 5;
    public GameObject swordCast;
    public Animator animationController;
    public AnimationClip attackClip;

    float timeBetweenAttack = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        tree = new BehaviorTree();
        Sequence idleLoop = new Sequence("Idle or Act");
        Leaf detectPlayer = new Leaf("Player In Sight", PlayerInSight);
        Selector attacking = new Selector("Do Things to Attack");
        Leaf playerTooFar = new Leaf("Player Too Far", Approach);
        Sequence attackSequence = new Sequence("Attack the Player");
        Leaf melee = new Leaf("Swing Sword", MeleeAttack);

        idleLoop.AddChild(detectPlayer);
        idleLoop.AddChild(attacking);
        attacking.AddChild(playerTooFar);
        attacking.AddChild(attackSequence);
        attackSequence.AddChild(melee);
        tree.AddChild(idleLoop);

        tree.PrintTree();
        attackSpeed = attackClip.length;
        swordCast.GetComponent<DamagePlayer>().damageValue = damage;
        swordCast.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
        {
            treeStatus = tree.Process();
        }

        if (treeStatus == Node.Status.SUCCESS)
        {
            treeStatus = Node.Status.RUNNING;
        }

        timeBetweenAttack += Time.deltaTime;
        animationController.SetFloat("Movement", agent.velocity.magnitude / agent.speed);
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

    public Node.Status Approach()
    {
        AnimatorStateInfo state = animationController.GetCurrentAnimatorStateInfo(0);
        if ((transform.position - player.position).magnitude > attackRange && !state.IsTag("Attacking"))
        {
            GoToLocation(player.position);
            return Node.Status.RUNNING;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status MeleeAttack()
    {
        GoToLocation(transform.position);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (timeBetweenAttack >= attackSpeed)
        {
            animationController.SetTrigger("Attacking");
            timeBetweenAttack = 0;
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public void ToggleSwordCast()
    {
        swordCast.SetActive(!swordCast.activeSelf);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        if (!agent.isOnNavMesh)
        {
            return Node.Status.FAILURE;
        }
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
