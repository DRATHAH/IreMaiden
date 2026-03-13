using System.Collections;
using UnityEngine;

public class ArcherAttack : EnemyAttack
{
    //Transform for the part where the bullets come out
    private Transform aimingNode;

    //Transform for the player
    private GameObject player;

    //Reference to the projectile prefab
    public GameObject arrow;

    //Reference to the movement script
    private ArcherMovement archerMovement;

    //Windup frames before shooting
    public float windup;

    //Frames after shooting before enemy can move again
    public float winddown;

    //Cooldown Vars

    //Maximum time before enemy can attack again
    public float maxCooldown;

    //Timer that determines when enemy can attack again
    private float cooldown;

    //Reference to Coroutine for attack that does the attacking sequence
    private IEnumerator attack;


    void Start()
    {
        //Define player
        player = GameObject.Find("Player");

        //Define bullet spawn
        aimingNode = this.GetComponentInChildren<AimingNode>().transform;

        //Define movement script
        archerMovement = this.GetComponent<ArcherMovement>();
    }

    void Update()
    {
        //Decreases the cooldown if above 0, if statement is to avoid underflow errors
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        //Check if the attacking coroutine is already running
        if (attack == null)
        {
            //Check if the cooldown lets you attack
            if (cooldown <= 0)
            {
                //Run the attack coroutine
                attack = AttackRoutine();
                StartCoroutine(attack);
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        //Windup
            //Set the attacking bool to be true
        archerMovement.attacking = true;

            //Start the windup frames
        yield return new WaitForSeconds(windup);
        
        //Attack
            //Create Projectile
            //*****ROTATION CODE NEEDS TO BE FIXED*********
        GameObject projectile = Instantiate(arrow, aimingNode.position, Quaternion.Euler(transform.rotation.x + aimingNode.rotation.x + 90, aimingNode.rotation.y, aimingNode.rotation.z));
        
            //Get Projectile Script
        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            //Calculate where to shoot
        Vector3 distance = Vector3.zero;
        distance.x = (player.transform.position.x - aimingNode.position.x);
        distance.y = (player.transform.position.y - aimingNode.position.y);
        distance.z = (player.transform.position.z - aimingNode.position.z);
        distance = distance.normalized;

            //shoot
        projectileScript.velocity = distance;

        //Winddown
            //Wait the winddown frames
        yield return new WaitForSeconds(winddown);

        //Start the cooldown before the enemy can attack again
        cooldown = maxCooldown;

        //Set the attacking bool to be false
        archerMovement.attacking = false;

        //Set the coroutine to be null to reset the enemy attacks
        attack = null;
    }

    //Function to stop the enemy from attacking, will be useful later for when dying is programmed in
    public override void StopAttacking()
    {
        if (attack != null)
        {
            StopCoroutine(attack);
            cooldown = maxCooldown;
            archerMovement.attacking = false;
            attack = null;
        }
    }
}
