using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : DamageableCharacter
{

    private Vector3 spawnPoint;

    private AudioSource SFX;
    public AudioClip[] DeathSounds;

    public WaveSpawner WaveSource; //Storing the arena this enemy spawns from

    private Animator anim;

    void Start()
    {
        spawnPoint = transform.position; //Set the point where the enemy should respawn if respawned.
        SFX = this.gameObject.GetComponentInChildren<AudioSource>();
       // anim = this.GetComponent<Animator>();
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
        if (WaveSource != null)
        {
            SFXManager.PlaySound(DeathSounds[Random.Range(0, DeathSounds.Length)], this.transform.position);
            if (this.gameObject.TryGetComponent<EnemyAttack>(out EnemyAttack attackingScript))
            {
                attackingScript.StopAttacking();
            }
            Health = maxHealth;
            targetable = true;
            transform.position = spawnPoint;
            WaveSource.EnemyDied();
            gameObject.SetActive(false);
        }
        else
        {
            SFXManager.PlaySound(DeathSounds[Random.Range(0, DeathSounds.Length)], this.transform.position);
            Destroy(this.gameObject);
        }
    }

    public void ResetEnemy() //Func to reset enemy pos and health upon player death
    {
        if (this.gameObject.TryGetComponent<EnemyAttack>(out EnemyAttack attackingScript))
        {
            attackingScript.StopAttacking();
        }
        Health = maxHealth;
        targetable = true;
        transform.position = spawnPoint;
        gameObject.SetActive(false);
    }

}
