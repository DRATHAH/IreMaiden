using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ProjectileType
    {
        bullet,
        explosion,
    }
    public ProjectileType type = ProjectileType.bullet;
    public int damage = 1;
    public GameObject explosionPrefab;
    public float explosionRadius = 5;
    public Rigidbody rb;

    bool hasHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(bool isExplosion, int dmg, float speed, Vector3 direction, bool gravity, float radius)
    {
        if (isExplosion)
        {
            type = ProjectileType.explosion;
        }
        else
        {
            type = ProjectileType.bullet;
        }

        damage = dmg;
        rb.linearVelocity = direction * speed;
        rb.useGravity = gravity;
        explosionRadius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit)
        {
            if (type == ProjectileType.explosion)
            {
                hasHit = true;
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Explosion explode = explosion.GetComponent<Explosion>();
                explode.Initialize(damage, explosionRadius, 1000);
            }
            else
            {
                if (other.GetComponent<EnemyHealth>())
                {
                    hasHit = true;
                    other.GetComponent<EnemyHealth>().TakeDamage(damage, other.name);
                }
            }
        }

        Destroy(gameObject);
    }
}
