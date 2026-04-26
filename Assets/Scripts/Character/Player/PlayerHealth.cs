using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : DamageableCharacter
{
    //InvincibilityFrames
    public float maxInvTimer = 1f;
    public bool isInvincible = false;

    [HideInInspector]public GameManager gamemanager;
    public Slider HPSlider;


    void Start()
    {
        health = maxHealth;
        HPSlider.value = health;
    }

    void Update()
    {
    }

    public void ResetHealth()
    {
        health = maxHealth;
        HPSlider.value = health;
        targetable = true;
    }

    public override void OnHit(int damage, GameObject hit, bool limbDamage)
    {
        if (hit.name.Contains("Head") && limbDamage == true)
        {
            damage *= 3;
        }
        else if (hit.name.Contains("Limb") && limbDamage == true)
        {
            damage *= 2;
        }
        else
        {
            damage *= 1;
        }

        Health -= damage;
        HPSlider.value = health;
    }

    //Function to kill the player
    public override void RemoveCharacter()
    {
        gamemanager.PlayerDeath();
    }

}
