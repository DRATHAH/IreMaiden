using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
    public int Health
    {
        set
        {
            health = value;
            if (value > 0)
            {
                // hit animation
            }

            if (health <= 0 && Targetable)
            {
                Targetable = false;
                RemoveCharacter();
            }
        }

        get
        {
            return health;
        }
    }

    public bool Targetable
    {
        get { return targetable; }
        set
        {
            targetable = value;
        }
    }


    public int maxHealth = 10;
    public int health = 10;
    public float regenDelay = 2f;
    public float regenRate = 1f;

    public bool targetable = true;

    private void Start()
    {
        
    }

    public virtual void Recoil(Vector3 knockback, bool takesKnockBack)
    {
        // Code for applying knockback effects
    }

    public virtual void OnHit(int damage, GameObject hit, bool limbDamage)
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
    }

    public virtual void RemoveCharacter()
    {
        Destroy(gameObject);
    }
}
