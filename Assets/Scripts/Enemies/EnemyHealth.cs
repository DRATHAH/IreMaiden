using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  
    private float currentHealth; //float for the enemy's current health

    public float maxHealth; //float for the enemy's maximum health value

    private Vector3 spawnPoint;

    public GameObject[] bodyArray; //Array for storing body parts of an enemy for damage calcs

    public float[] limbDamageMultiplier; //Array for storing the damage multiplier given by body parts

    public WaveSpawner WaveSource; //Storing the arena this enemy spawns from


    void Start()
    {
        spawnPoint = transform.position; //Set the point where the enemy should respawn if respawned.
    }

    void OnEnable()
    {
        currentHealth = maxHealth; //Set current health = max health upon spawning in
    }

    public void TakeDamage(float amount, string location) //Function for taking and determining the amount of damage taken
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

    private void Die() //Death Function
    {
        if(WaveSource != null) //If spawned via a wave spawner
        {
            WaveSource.EnemyDied(); //Tell the wave spawner that its dead
            currentHealth = maxHealth; // Reset Enemy Health
            transform.position = spawnPoint; // Reset enemy position
            this.gameObject.SetActive(false); // Set the object to be inactive
        }
        else //If spawned manually
        {
            Destroy(this.gameObject); //Destroy the object upon death
        }
    }


    public void ResetEnemy() //Func to reset enemy pos and health upon player death
    {
        currentHealth = maxHealth;
        transform.position = spawnPoint;
        this.gameObject.SetActive(false);
    }
}
