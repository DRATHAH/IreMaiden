using UnityEngine;

public class EnemyHealth : DamageableCharacter
{

    private Vector3 spawnPoint;

    public GameObject[] bodyArray; //Array for storing body parts of an enemy for damage calcs

    public float[] limbDamageMultiplier; //Array for storing the damage multiplier given by body parts

    public WaveSpawner WaveSource; //Storing the arena this enemy spawns from


    void Start()
    {
        spawnPoint = transform.position; //Set the point where the enemy should respawn if respawned.
    }


    public void ResetEnemy() //Func to reset enemy pos and health upon player death
    {
        Health = maxHealth;
        transform.position = spawnPoint;
        gameObject.SetActive(false);
    }
}
