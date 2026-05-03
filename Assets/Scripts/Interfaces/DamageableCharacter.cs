using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

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
    public bool isGrounded = true;

    public bool targetable = true;

    private void Start()
    {
        
    }

    public virtual void Recoil(float knockback, Vector3 position, float radius, float upwardsMod, float stunTime)
    {
        if (GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        GetComponent<Rigidbody>().AddExplosionForce(knockback, position, 0, upwardsMod);

        //StartCoroutine(EnableNavAgent(stunTime));
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (GetComponent<NavMeshAgent>())
            {
                GetComponent<NavMeshAgent>().enabled = true;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public virtual void RemoveCharacter()
    {
        Destroy(gameObject);
    }

    public IEnumerator EnableNavAgent(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        if (GetComponent<NavMeshAgent>())
        {
            
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;


        }
    }
}
