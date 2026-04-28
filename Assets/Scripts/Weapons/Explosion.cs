using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 5f;
    public float explosionRadius = 5;
    public float delayLifetime = 5;
    public float stunDuration = 1;

    string ownerTag = "";

    private List<Transform> HitObjects = new List<Transform>();
    private List<Transform> KnockbackObjects = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Explode();
    }

    public void Explode()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, explosionRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            // Debug.Log(col.name);
            if(col.transform.root.tag == "Player")
            {
                if (col.transform.root.GetComponentInChildren<Rigidbody>() && !KnockbackObjects.Contains(col.transform.root))
                {
                    Debug.Log(col.transform.root);
                    KnockbackObjects.Add(col.transform.root);
                }
            }
            else if (col.transform.root.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                if (col.transform.root.GetComponentInChildren<Rigidbody>() && !KnockbackObjects.Contains(col.transform.root))
                {
                    KnockbackObjects.Add(col.transform.root);
                }
            }

            if (col.transform.root.GetComponentInChildren<DamageableCharacter>() && !col.transform.root.CompareTag(ownerTag) && !HitObjects.Contains(col.transform.root))
            {
                HitObjects.Add(col.transform.root);
            }
        }

        foreach (Transform damageable in HitObjects)
        {
            damageable.GetComponentInChildren<DamageableCharacter>().OnHit(damage, damageable.transform.root.gameObject, false);
            Debug.Log("Damaged " + damageable.transform.name);
        }
        foreach(Transform obj in KnockbackObjects)
        {
            if (obj.TryGetComponent<DamageableCharacter>(out DamageableCharacter character))
            {
                character.Recoil(knockback, transform.position, explosionRadius, 2, stunDuration);
            }
            else
            {
                obj.GetComponentInChildren<Rigidbody>().AddExplosionForce(knockback, transform.position, 0, 2);
            }
        }

        HitObjects.Clear();
        KnockbackObjects.Clear();
        StartCoroutine(DelayDestroy());
    }

    public void Initialize(int dmg, float radius, float knockbackForce, string owner)
    {
        damage = dmg;
        explosionRadius = radius;
        knockback = knockbackForce;
        ownerTag = owner;
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(delayLifetime);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
