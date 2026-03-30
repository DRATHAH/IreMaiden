using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged(); // When called, calls every method subscribed
    public OnItemChanged onItemChangedCallback;

    public int spellSpace = 4;
    public List<SpellAbility> spells = new List<SpellAbility>();

    public bool Add(SpellAbility ability)
    {
        if (spells.Count < spellSpace && !spells.Contains(ability))
        {
            spells.Add(ability);
        }
        else if (spells.Count >= spellSpace || spells.Contains(ability))
        {
            return false;
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true;
    }

    public void Remove(SpellAbility ability)
    {
        if (spells.Contains(ability))
        {
            spells.Remove(ability);
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void ClearInventory()
    {
        spells.Clear();
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
