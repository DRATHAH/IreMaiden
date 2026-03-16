using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public int GunDamagePrimary1;

    public float spellOffset = 1;

    public List<SpellAbility> spells = new List<SpellAbility>();
    public Dictionary<SpellAbility, float> spellCooldowns = new Dictionary<SpellAbility, float>(); // Keeps track of the cooldowns of all spells

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(SpellAbility spell in spells)
        {
            spellCooldowns.Add(spell, spell.cooldown);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spellCooldowns[spells[0]] <= spells[0].cooldown)
        {
            spellCooldowns[spells[0]] += Time.deltaTime;
        }

        //Check if the player can fire
        if(fireCooldownPrimary1 > 0)
        {
            //If no reduce cooldown
            fireCooldownPrimary1 -= Time.deltaTime;
        }
        else
        {
            //If it can check for input
            if (Input.GetKey(KeyCode.Mouse0))
            {
                ShootWeaponPrimary1();
            }
        }

        if (Input.GetKey(KeyCode.Mouse1) && spellCooldowns[spells[0]] >= spells[0].cooldown)
        {
            // Temporary code
            Fireball(0);
            spellCooldowns[spells[0]] = 0;
        }
    }

    //Function for the primary fire of weapon 1
    void ShootWeaponPrimary1()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGunRangePrimary1))
        {
            DamageableCharacter enemyHealth = hit.collider.gameObject.GetComponentInParent<DamageableCharacter>();
            if (enemyHealth != null && hit.collider.transform.root.CompareTag("Enemy"))
            {
                enemyHealth.OnHit(GunDamagePrimary1, hit.collider.gameObject, true);
                fireCooldownPrimary1 = maxFireCooldownPrimary1;
                Debug.Log("Shot " + hit.collider.name);
            }
        }
    }

    //Function for the secondary fire of weapon 1
    void ShootWeaponSecondary1()
    {

    }

    #region Spells
    // Make sure every method has the same name as the spell

    void Fireball(int index)
    {
        GameObject projectile = Instantiate(spells[index].spellPrefab, Camera.main.transform.position + (Camera.main.transform.forward * spellOffset), Quaternion.identity);
        Projectile prjScript = projectile.GetComponentInParent<Projectile>();
        if (prjScript != null)
        {
            prjScript.Initialize(true, spells[index].damage, 50, Camera.main.transform.forward, false, 10, transform.tag);
        }
    }

    #endregion
}
