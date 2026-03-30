using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

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

    private Inventory inventory;
    int spellIndex; // Index of the spell currently being cast
    bool finishedLoadout = false; // Whether the player's loadout has finished updating

    public Dictionary<SpellAbility, float> spellCooldowns = new Dictionary<SpellAbility, float>(); // Keeps track of the cooldowns of all spells


    private List<Sprite> spellIcon = new List<Sprite>();
    public Image[] BookIcons;
    public Slider[] CooldownSliders;
    private int spellIndex2 = 1; //Index of spell in 2nd slot
    private int spellIndex3 = 2; //Index of spell in 3rd slot
    private int spellIndex4 = 3; //Index of spell in 4th slot

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = Inventory.instance;

        for (int i = 0; i < inventory.spells.Count; i++)
        {
            spellCooldowns.Add(inventory.spells[i], 0);
            spellIcon.Add(inventory.spells[i].icon);
            UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), this, inventory.spells[i].name);
            spellActions[i].AddListener(delegate { action(); });
            BookIcons[i].sprite = spellIcon[i];
            CooldownSliders[i].maxValue = inventory.spells[i].cooldown;
        }
        for(int i = inventory.spells.Count; i < BookIcons.Length; i++)
        {
            BookIcons[i].transform.parent.gameObject.SetActive(false);
        }
        spellIndex = 0;
        finishedLoadout = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedLoadout && spellCooldowns[inventory.spells[0]] <= inventory.spells[0].cooldown)
        {
            spellCooldowns[inventory.spells[0]] -= Time.deltaTime;
        }
        if(finishedLoadout && spellCooldowns.Count > 1 && spellCooldowns[inventory.spells[1]] <= inventory.spells[1].cooldown)
        {
            spellCooldowns[inventory.spells[1]] -= Time.deltaTime;
        }
        if (finishedLoadout && spellCooldowns.Count > 2 && spellCooldowns[inventory.spells[2]] <= inventory.spells[1].cooldown)
        {
            spellCooldowns[inventory.spells[2]] -= Time.deltaTime;
        }
        if (finishedLoadout && spellCooldowns.Count > 3 && spellCooldowns[inventory.spells[3]] <= inventory.spells[1].cooldown)
        {
            spellCooldowns[inventory.spells[3]] -= Time.deltaTime;
        }
        //Check if the player can fire
        if (fireCooldownPrimary1 > 0)
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

        if (Input.GetKey(KeyCode.Mouse1) && spellCooldowns[inventory.spells[spellIndex]] <= 0)
        {
            ShootWeaponSecondary1();
        }


        if (Input.GetKey(KeyCode.Alpha1) && spellIcon.Count > 0)
        {
            // Temporary code
            //spellActions[0].Invoke();
            spellIndex = 0;
            spellIndex2 = 1;
            spellIndex3 = 2;
            spellIndex4 = 3;

            BookIcons[0].sprite = spellIcon[spellIndex];
            CooldownSliders[0].maxValue = inventory.spells[spellIndex].cooldown;

            if(spellIcon.Count > 1)
            {
                BookIcons[1].sprite = spellIcon[spellIndex2];
                CooldownSliders[1].maxValue = inventory.spells[spellIndex2].cooldown;
            }
            if (spellIcon.Count > 2)
            {
                BookIcons[2].sprite = spellIcon[spellIndex3];
                CooldownSliders[2].maxValue = inventory.spells[spellIndex3].cooldown;
            }
            if (spellIcon.Count > 3)
            {
                BookIcons[3].sprite = spellIcon[spellIndex4];
                CooldownSliders[3].maxValue = inventory.spells[spellIndex4].cooldown;
            }
            //spellCooldowns[inventory.spells[0]] = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && spellIcon.Count > 1)
        {
            // Temporary code
            //spellActions[1].Invoke();
            spellIndex = 1;
            spellIndex2 = 0;
            spellIndex3 = 2;
            spellIndex4 = 3;

            BookIcons[0].sprite = spellIcon[spellIndex];
            CooldownSliders[0].maxValue = inventory.spells[spellIndex].cooldown;
            
            BookIcons[1].sprite = spellIcon[spellIndex2];
            CooldownSliders[1].maxValue = inventory.spells[spellIndex2].cooldown;

            if (spellIcon.Count > 2)
            {
                BookIcons[2].sprite = spellIcon[spellIndex3];
                CooldownSliders[2].maxValue = inventory.spells[spellIndex3].cooldown;
            }
            if (spellIcon.Count > 3)
            {
                BookIcons[3].sprite = spellIcon[spellIndex4];
                CooldownSliders[3].maxValue = inventory.spells[spellIndex4].cooldown;
            }

            //spellCooldowns[inventory.spells[1]] = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha3) && spellIcon.Count > 2)
        {
            // Temporary code
            spellIndex = 2;
            spellIndex2 = 0;
            spellIndex3 = 1;
            spellIndex4 = 3;

            BookIcons[0].sprite = spellIcon[spellIndex];
            CooldownSliders[0].maxValue = inventory.spells[spellIndex].cooldown;

            BookIcons[1].sprite = spellIcon[spellIndex2];
            CooldownSliders[1].maxValue = inventory.spells[spellIndex2].cooldown;

            BookIcons[2].sprite = spellIcon[spellIndex3];
            CooldownSliders[2].maxValue = inventory.spells[spellIndex3].cooldown;

            if (spellIcon.Count > 3)
            {
                BookIcons[3].sprite = spellIcon[spellIndex4];
                CooldownSliders[3].maxValue = inventory.spells[spellIndex4].cooldown;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha4) && spellIcon.Count > 3)
        {
            // Temporary code
            spellIndex = 3;
            spellIndex2 = 0;
            spellIndex3 = 1;
            spellIndex4 = 2;

            BookIcons[0].sprite = spellIcon[spellIndex];
            CooldownSliders[0].maxValue = inventory.spells[spellIndex].cooldown;

            BookIcons[1].sprite = spellIcon[spellIndex2];
            CooldownSliders[1].maxValue = inventory.spells[spellIndex2].cooldown;

            BookIcons[2].sprite = spellIcon[spellIndex3];
            CooldownSliders[2].maxValue = inventory.spells[spellIndex3].cooldown;

            BookIcons[3].sprite = spellIcon[spellIndex4];
            CooldownSliders[3].maxValue = inventory.spells[spellIndex4].cooldown;
        }

        CooldownSliders[0].value = spellCooldowns[inventory.spells[spellIndex]];
        if(spellIcon.Count > 1)
        {
            CooldownSliders[1].value = spellCooldowns[inventory.spells[spellIndex2]];
        }
        if(spellIcon.Count > 2)
        {
            CooldownSliders[2].value = spellCooldowns[inventory.spells[spellIndex3]];
        }
        if (spellIcon.Count > 3)
        {
            CooldownSliders[3].value = spellCooldowns[inventory.spells[spellIndex4]];
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
        spellActions[spellIndex].Invoke();
        spellCooldowns[inventory.spells[spellIndex]] = inventory.spells[spellIndex].cooldown;
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
        RaycastHit ray;
        if (Physics.Raycast(Camera.main.transform.position + (Camera.main.transform.forward * spellOffset), Camera.main.transform.forward, out ray, 100))
        {
            Instantiate(inventory.spells[spellIndex].spellPrefab, ray.point, Quaternion.identity);
        }
    }

    void ExtraSpell()//Added for testing
    {
        Debug.Log("Extra");
    }

    void Nothing()//Added for testing
    {
        Debug.Log("Nothing");
    }
    #endregion
}
