using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //Projectile's rigidbody
    private Rigidbody rb;

    //Projectile's velocity given by attacking script
    public Vector3 velocity;

    //How fast the projectile will move
    public int projectileSpeed;


    void Start()
    {
        //Rigidbody definition
        rb = this.GetComponent<Rigidbody>();

        //If there is a velocity given set the rigidbody's velocity to that
        if(velocity != null)
        {
            rb.linearVelocity = velocity * projectileSpeed;
        }

    }

    void OnTriggerStay(Collider other)
    {
        //Destroy the projectile if it comes into contact with another object that isn't a projectile
        if(other.tag != "Projectile")
        {
            Destroy(this.gameObject);
        }
    }
}
