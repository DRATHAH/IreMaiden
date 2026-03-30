using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 5f;
    public float explosionRadius = 5;
    public float delayLifetime = 5;

    string ownerTag = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Collider col in Physics.OverlapSphere(transform.position, explosionRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
           // Debug.Log(col.name);
            if (col.transform.root.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                body.AddExplosionForce(knockback, transform.position, explosionRadius, 2);
            }

            if (col.transform.root.TryGetComponent<DamageableCharacter>(out DamageableCharacter character) && !col.transform.root.CompareTag(ownerTag))
            {
                character.OnHit(damage, col.transform.root.gameObject, false);
                character.Recoil(Vector3.zero, false);
                Debug.Log("Damaged " + col.transform.name);
            }
        }

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
}
