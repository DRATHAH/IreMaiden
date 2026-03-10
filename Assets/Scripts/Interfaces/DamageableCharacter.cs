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

    bool targetable = true;

    private void Start()
    {
        
    }

    public void OnHit(int damage, Vector3 knockback)
    {
        
    }

    public void OnHit(int damage, GameObject hit)
    {

    }

    public virtual void RemoveCharacter()
    {

    }
}
