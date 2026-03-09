using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    public float damage = 1;
    public float knockback = 5f;
    public float explosionRadius = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Collider col in Physics.OverlapSphere(transform.position, explosionRadius))
        {
            if (col.GetComponent<Rigidbody>())
            {
                col.GetComponent<Rigidbody>().AddExplosionForce(knockback, transform.position, explosionRadius);
            }
        }

        StartCoroutine(DelayDestroy());
    }

    public void Initialize(int dmg, float radius, float knockbackForce)
    {
        damage = dmg;
        explosionRadius = radius;
        knockback = knockbackForce;
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
