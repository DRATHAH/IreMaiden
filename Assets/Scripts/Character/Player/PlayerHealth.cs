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

    //Function to kill the player
    public override void RemoveCharacter()
    {
        gamemanager.PlayerDeath();
    }

}
