using UnityEngine;

public class PlayerHealth : DamageableCharacter
{
    //InvincibilityFrames
    public float maxInvTimer = 1f;
    public bool isInvincible = false;

    [HideInInspector]public GameManager gamemanager;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
    }

    public void ResetHealth()
    {
        health = maxHealth;
        targetable = true;
    }

    //Function to kill the player
    public override void RemoveCharacter()
    {
        gamemanager.PlayerDeath();
    }

}
