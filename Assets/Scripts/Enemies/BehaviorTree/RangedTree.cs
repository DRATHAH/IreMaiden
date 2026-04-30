using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using IM;

public class RangedTree : DamageableCharacter
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
    public GameObject soundParticle;
    public AudioSource fireSound;
    public Transform arrowSpawn;

    float timeBetweenAttack = 0;
    public AudioClip[] DeathSounds;
    private Vector3 spawnPoint;

    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = this.transform.position;
        anim = GetComponentInChildren<Animator>();

        tree = new BehaviorTree();
        Sequence idleLoop = new Sequence("Idle or Act");
        Leaf detectPlayer = new Leaf("Player In Sight", PlayerInSight);
        Selector rangeIncrement = new Selector("Get Player In Range");
        Leaf playerTooClose = new Leaf("Player Too Close", Retreat);
        Sequence attacking = new Sequence("Do Things to Attack");
        Leaf playerTooFar = new Leaf("Player Too Far", Approach);
        Sequence attackSequence = new Sequence("Attack the Player");
        Leaf shoot = new Leaf("Shoot the Player", RangedAttack);
        Leaf reposition = new Leaf("Reposition After Attack", Reposition);

        idleLoop.AddChild(detectPlayer);
        idleLoop.AddChild(rangeIncrement);
        rangeIncrement.AddChild(playerTooClose);
        attacking.AddChild(playerTooFar);
        attacking.AddChild(attackSequence);
        rangeIncrement.AddChild(attacking);
        attackSequence.AddChild(shoot);
        attackSequence.AddChild(reposition);
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

        if (treeStatus == Node.Status.SUCCESS)
        {
            treeStatus = Node.Status.RUNNING;
        }

        anim.SetFloat("Movement", agent.velocity.magnitude);

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
        if (NavMesh.SamplePosition(transform.position - (transform.forward * retreatRange), out NavMeshHit hit, retreatRange / 2, NavMesh.AllAreas))
        {
            if ((transform.position - player.position).magnitude < retreatRange)
            {
                anim.SetBool("Retreat", true);
                agent.updateRotation = false;
                return GoToLocation(hit.position);
            }
            return Node.Status.FAILURE;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status Approach()
    {
        agent.updateRotation = true;
        if ((transform.position - player.position).magnitude > attackRange && agent.isOnNavMesh)
        {
            anim.SetBool("Retreat", false);
            agent.SetDestination(player.position);
            return Node.Status.RUNNING;
        }
        else if ((transform.position - player.position).magnitude <= attackRange && agent.isOnNavMesh)
        {
            GoToLocation(transform.position);
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status RangedAttack()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(transform.position);
        }
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        Ray ray = new Ray(transform.position, (player.position - transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.root.GetComponentInChildren<PlayerHealth>())
            {
                if (timeBetweenAttack >= attackSpeed)
                {
                    anim.SetTrigger("Attack");
                    Debug.Log("attack");
                    GameObject arrow = Instantiate(projectile, arrowSpawn.position + (transform.forward * 0.5f), Quaternion.identity);
                    Projectile proj = arrow.GetComponent<Projectile>();
                    proj.Initialize(false, damage, arrowSpeed, (player.position - arrowSpawn.position).normalized, false, 0, "Enemy");
                    timeBetweenAttack = 0;
                    GameObject sound = Instantiate(soundParticle, transform.position, Quaternion.identity);
                    sound.GetComponent<SoundObject>().Initialize(fireSound);
                    return Node.Status.SUCCESS;
                }
            }
        }
        return Node.Status.FAILURE;
    }

    public Node.Status Reposition()
    {
        if ((transform.position - player.position).magnitude < retreatRange)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000, transform.position + transform.forward, 1);
            return Node.Status.RUNNING;
        }
        else
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(transform.position);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
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
        Gizmos.DrawWireSphere(transform.position, retreatRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //public override void RemoveCharacter()
    //{
    //    if (WaveSource != null)
    //    {
    //        SFXManager.PlaySound(DeathSounds[Random.Range(0, DeathSounds.Length)], this.transform.position);
    //        Health = maxHealth;
    //        targetable = true;
    //        transform.position = spawnPoint;
    //        WaveSource.EnemyDied();
    //        gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        SFXManager.PlaySound(DeathSounds[Random.Range(0, DeathSounds.Length)], this.transform.position);
    //        Destroy(this.gameObject);
    //    }
    //}

}
