using UnityEngine;

public class PlayerWeaponry : MonoBehaviour
{
    //Vars for the primary fire of weapon 1
       
        //Range for primary fire 1
    public float maxGunRangePrimary1;
        
        //Current cooldown for primary fire 1
    private float fireCooldownPrimary1;

        //Maximum cooldown for primary fire 1
    public float maxFireCooldownPrimary1;
        
        //Damage Dealt by primary fire 1
    public float GunDamagePrimary1;

        //Enemy health being referenced by the damage dealing function
    private EnemyHealth enemyHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player can fire
        if(fireCooldownPrimary1 > 0)
        {
            //If no reduce cooldown
            fireCooldownPrimary1 -= Time.deltaTime;
        }
        else
        {
            //If it can check for input
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootWeaponPrimary1();
            }
        }
    }

    //Function for the primary fire of weapon 1
    void ShootWeaponPrimary1()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGunRangePrimary1))
        {
            enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(GunDamagePrimary1, hit.collider.name);
                fireCooldownPrimary1 = maxFireCooldownPrimary1;
            }
        }
    }

    //Function for the secondary fire of weapon 1
    void ShootWeaponSecondary1()
    {

    }
}
