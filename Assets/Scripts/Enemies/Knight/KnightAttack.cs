using System.Collections;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    //Attacking Vars
        //Windup frames before hitbox is active
    public float windup;
        //Frames where attacking hitbox is actually active
    public float activeAttack;
        //Frames after the attack before enemy can move again
    public float winddown;
    
    //Cooldown Vars
        //Maximum time before enemy can attack again
    public float maxCooldown;
        //Timer that determines when enemy can attack again
    private float cooldown;

    //Gameobject reference for the attacking hitbox
    public GameObject attackBox;

    //Reference to Coroutine for attack that does the attacking sequence
    private IEnumerator attack;

    //Reference to the Enemy's movement script
    private KnightMovement knightMovement;

    private Rigidbody rb;

    void Start()
    {
        //Define Knight Movement
        knightMovement = this.GetComponentInParent<KnightMovement>();

        //Define the hitbox
        attackBox = this.GetComponentInChildren<DamagePlayer>().gameObject;

        //Set the hitbox to inactive after it is defined (If it starts disabled the prior statement can't define properly.
        attackBox.SetActive(false);

        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Decreases the cooldown if above 0, if statement is to avoid underflow errors
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        //Check if the cooldown lets you attack
        if (cooldown <= 0)
        {
            //Run the attack coroutine
            if (attack == null)
            {
                attack = AttackRoutine();
                StartCoroutine(attack);
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        //Windup
            //Set the attacking bool to be true
        knightMovement.attacking = true;
            //Start the windup frames
        yield return new WaitForSeconds(windup);
        //Attack
            //Set the attacking hitbox to be active
        attackBox.SetActive(true);

        yield return new WaitForSeconds(activeAttack); //Wait for the frames that the hitbox should be active for
        //Winddown
        //Turn the attacking hitbox off
        attackBox.SetActive(false);
            //Wait the winddown frames
        yield return new WaitForSeconds(winddown);
            //Start the cooldown before the enemy can attack again
        cooldown = maxCooldown;
            //Set the attacking bool to be false
        knightMovement.attacking = false;
            //Set the coroutine to be null to reset the enemy attacks
        attack = null;
    }

    //Function to stop the enemy from attacking, will be useful later for when dying is programmed in
    void StopAttacking()
    {
        if(attack != null)
        {
            StopCoroutine(attack);
            attack = null;
        }
    }
}
