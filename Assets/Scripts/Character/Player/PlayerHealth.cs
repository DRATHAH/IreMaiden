using UnityEngine;

public class PlayerHealth : DamageableCharacter
{
    //InvincibilityFrames
    public float invincibilityTimer;
    public float maxInvTimer = 3f;
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
