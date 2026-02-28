using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Player Health
    public int playerHealth;
    public int maxHealth = 100;


    //InvincibilityFrames
    public float invincibilityTimer;
    public float maxInvTimer = 3f;
    public bool isInvincible = false;

    void Start()
    {
        /*Set this to be equal to max health upon starting a level.
         * Can be changed to reference something else if we need to have multiple scenes within one level.
        */
        playerHealth = maxHealth;
    }

    void Update()
    {
        //Invincibility Frames Management
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            isInvincible = true;
        }
        else
        {
            isInvincible = false;
        }
    }

    //Function to handle taking damage from an enemy
    public void TakeDamage(int damageAmount)
    {
        if(isInvincible == false)
        {
            //Subtract from player health
            playerHealth -= damageAmount;
            //If playerhealth is less than 0 kill them
            if(playerHealth <= 0)
            {
                Die();
                return;
            }
            //Set invincibilityTimer
            invincibilityTimer = maxInvTimer;
        }
    }

    //Function to handle healing the player
    public void HealDamage(int healAmount)
    {
        if((playerHealth + healAmount) < maxHealth)
        {
            playerHealth += healAmount;
        }
        else if(playerHealth + healAmount >= maxHealth)
        {
            playerHealth = maxHealth;
        }
    }

    /*Function to kill the player
     * THIS IS INCOMPLETE! CURRENTLY JUST A TEST VERSION!
     * FINISH THIS ONCE WE KNOW HOW WE WANT TO HANDLE PLAYER DEATH!
    */
    private void Die()
    {
        Debug.Log("Dead");
        playerHealth = maxHealth;
    }

}
