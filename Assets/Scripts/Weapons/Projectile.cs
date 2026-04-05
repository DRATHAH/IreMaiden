using System.Collections;
using System.Collections.Generic;
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
    public float explosionForce = 10000f;
    public Rigidbody rb;
    public string ownerTag;

    bool hasHit = false;

    private PlaySFX playSFX;
    public AudioClip FireballEndingSFX;
    [SerializeField, Range(-80, 0)] private float FireballVolume;

    private Vector3 StartingPoint;
    public Transform Source;
    private Vector3 EndingPoint;

    private List<Transform> HitObjects = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playSFX = this.GetComponent<PlaySFX>();
    }

    public void Initialize(bool isExplosion, int dmg, float speed, Vector3 direction, bool gravity, float radius, string owner)
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
        ownerTag = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit && !collision.transform.CompareTag(ownerTag))
        {
            if (type == ProjectileType.explosion)
            {
                hasHit = true;
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                Explosion explode = explosion.GetComponent<Explosion>();
                explode.Initialize(damage, explosionRadius, explosionForce, ownerTag);
                EndingPoint = transform.position;
                playSFX.PlaySound3D(FireballEndingSFX, Source.position, EndingPoint, FireballVolume);
            }
            else
            {
                if (collision.transform.GetComponent<DamageableCharacter>() && collision.transform.CompareTag("Enemy") && HitObjects.Contains(collision.transform.root) == false)
                {
                    HitObjects.Add(collision.transform.root);
                    hasHit = true;
                    collision.transform.GetComponent<DamageableCharacter>().OnHit(damage, collision.transform.gameObject, false);
                }
            }

            Destroy(gameObject);
        }
    }
}
