using UnityEngine;

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
    }

    public void Initialize(int dmg, float radius, float knockbackForce)
    {
        damage = dmg;
        explosionRadius = radius;
        knockback = knockbackForce;
    }
}
