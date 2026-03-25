using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

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

    [Header("Spell Events")] // These run when you press the num keys. Make sure to have 4, even if they don't do anything
    public List<UnityEvent> spellActions;

    public float spellOffset = 1;

    Inventory inventory;
    int spellIndex; // Index of the spell currently being cast
    bool finishedLoadout = false; // Whether the player's loadout has finished updating

    public Dictionary<SpellAbility, float> spellCooldowns = new Dictionary<SpellAbility, float>(); // Keeps track of the cooldowns of all spells

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = Inventory.instance;

        for (int i = 0; i < inventory.spells.Count; i++)
        {
            spellCooldowns.Add(inventory.spells[i], inventory.spells[i].cooldown);

            UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), this, inventory.spells[i].name);
            spellActions[i].AddListener(delegate { action(); });
        }
        finishedLoadout = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedLoadout && spellCooldowns[inventory.spells[0]] <= inventory.spells[0].cooldown)
        {
            spellCooldowns[inventory.spells[0]] += Time.deltaTime;
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

        if (Input.GetKey(KeyCode.Alpha1) && spellCooldowns[inventory.spells[0]] >= inventory.spells[0].cooldown)
        {
            // Temporary code
            spellActions[0].Invoke();
            spellIndex = 0;
            spellCooldowns[inventory.spells[0]] = 0;
        }
        if (Input.GetKey(KeyCode.Alpha2) && spellCooldowns[inventory.spells[1]] >= inventory.spells[1].cooldown)
        {
            // Temporary code
            spellActions[1].Invoke();
            spellIndex = 1;
            spellCooldowns[inventory.spells[1]] = 0;
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

    void Fireball()
    {
        GameObject projectile = Instantiate(inventory.spells[spellIndex].spellPrefab, Camera.main.transform.position + (Camera.main.transform.forward * spellOffset), Quaternion.identity);
        Projectile prjScript = projectile.GetComponentInParent<Projectile>();
        if (prjScript != null)
        {
            prjScript.Initialize(true, inventory.spells[spellIndex].damage, 50, Camera.main.transform.forward, false, 10, transform.tag);
        }
    }

    void Hand()
    {
        Debug.Log("Casted hand");
    }
    #endregion
}
