using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    public List<string> spellSelection = new List<string>();//Strings of spell names to call functions
    public List<SpellAbility> spells = new List<SpellAbility>();
    public Dictionary<SpellAbility, float> spellCooldowns = new Dictionary<SpellAbility, float>(); // Keeps track of the cooldowns of all spells
    public int CurrentSpellIndex = 0; //Spell being called

    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI spellCooldownText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentSpellIndex = 0;
        foreach (SpellAbility spell in spells)
        {
            spellSelection.Add(spell.name);
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
        if(spells.Count > 1 && spellCooldowns[spells[1]] <= spells[1].cooldown)
        {
            spellCooldowns[spells[1]] += Time.deltaTime;
        }
        if (spells.Count > 2 && spellCooldowns[spells[2]] <= spells[2].cooldown)
        {
            spellCooldowns[spells[2]] += Time.deltaTime;
        }

        spellNameText.text = "Spell: " + spells[CurrentSpellIndex].name;
        spellCooldownText.text = "Cooldown: " + Mathf.Ceil(spellCooldowns[spells[CurrentSpellIndex]]);


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

        if (Input.GetKey(KeyCode.Alpha1))
        {
            CurrentSpellIndex = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && spells.Count > 2)
        {
            CurrentSpellIndex = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && spells.Count > 3)
        {
            CurrentSpellIndex = 2;
        }


        if (Input.GetKey(KeyCode.Mouse1) && CurrentSpellIndex <= spells.Count && spellCooldowns[spells[CurrentSpellIndex]] >= spells[CurrentSpellIndex].cooldown)
        {
            // Temporary code
            ShootWeaponSecondary1();
            //Fireball(0);
            spellCooldowns[spells[CurrentSpellIndex]] = 0;
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
       Invoke(spellSelection[CurrentSpellIndex], 0f);
    }

    #region Spells
    // Make sure every method has the same name as the spell

    void Fireball(/*int index*/)
    {
        GameObject projectile = Instantiate(spells[CurrentSpellIndex].spellPrefab, Camera.main.transform.position + (Camera.main.transform.forward * spellOffset), Quaternion.identity);
        Projectile prjScript = projectile.GetComponentInParent<Projectile>();
        if (prjScript != null)
        {
            prjScript.Initialize(true, spells[CurrentSpellIndex].damage, 50, Camera.main.transform.forward, false, 10, transform.tag);
        }
    }

    #endregion
}
