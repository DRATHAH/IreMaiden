using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //float for the enemy's current health
    private float currentHealth;

    //float for the enemy's maximum health value
    public float maxHealth;

    //Array for storing body parts of an enemy for damage calcs
    public GameObject[] bodyArray;

    //Array for storing the damage multiplier given by body parts
    public float[] limbDamageMultiplier;

    void OnEnable()
    {
        //Set current health = max health upon spawning in
        currentHealth = maxHealth;
    }

    //Function for taking and determining the amount of damage taken
    public void TakeDamage(float amount, string location)
    {
        //Float determining the multiplier given for attacking enemy body parts
        float damageMutliplier = 0;

        //Check the location that the damage is being recieved to see what the modifier should be
        for (int i = 0; i < bodyArray.Length; i++)
        {
            if (bodyArray[i] != null && location == bodyArray[i].name)
            {
                //If the body part has been found assign the modifier from the limbDamageMultiplier
                if (i < limbDamageMultiplier.Length)
                {
                    damageMutliplier = limbDamageMultiplier[i];
                }
                else
                {
                    //Backup value of 1 in case something went wrong
                    damageMutliplier = 1;
                }
                    break;
            }
            else
            {
                //If not then do a base of 1 as a default value
                damageMutliplier = 1;
            }

        }

        //Set the current enemy health to the amount of damage dealt times the modifier
        currentHealth -= amount * damageMutliplier;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Temporary Death function
    //TO BE FINISHED LATER
    private void Die()
    {
        Debug.Log("Dead");
        currentHealth = maxHealth;
    }

}
