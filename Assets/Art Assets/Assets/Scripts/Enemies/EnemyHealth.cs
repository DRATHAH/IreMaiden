using UnityEngine;

public class EnemyHealth : DamageableCharacter
{

    private Vector3 spawnPoint;


    public WaveSpawner WaveSource; //Storing the arena this enemy spawns from


    void Start()
    {
        spawnPoint = transform.position; //Set the point where the enemy should respawn if respawned.
    }

    void Update()
    {
    }

    void OnEnable()
    {
        Health = maxHealth;
        targetable = true;
    }

    //Function to kill the player
    public override void RemoveCharacter()
    {
        if(WaveSource != null)
        {
            Health = maxHealth;
            targetable = true;
            transform.position = spawnPoint;
            WaveSource.EnemyDied();
            this.gameObject.GetComponent<EnemyAttack>().StopAttacking();
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ResetEnemy() //Func to reset enemy pos and health upon player death
    {
        this.gameObject.GetComponent<EnemyAttack>().StopAttacking();
        Health = maxHealth;
        targetable = true;
        transform.position = spawnPoint;
        gameObject.SetActive(false);
    }
}
