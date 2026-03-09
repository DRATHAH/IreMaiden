using UnityEngine;
/*
 * This script is used to hold data for different spells and their functions
*/

[CreateAssetMenu(fileName = "New Spell", menuName = "Inventory/Spell")] // Allows the creation of new spells by right-clicking in the explorer
public class SpellAbility : ScriptableObject
{
    new public string name = "New Spell"; // Name of the spell
    public Sprite icon = null; // Image of the spell
    [TextArea]
    public string desc; // Description of the spell
    public int damage; // How much damage the spell will do
    public float cooldown; // How long the spell will be on cooldown after being used
    public GameObject spellPrefab; // The prefab that is made when the spell is used
    public int spellId; // Id of the spell (for identification purposes)
}
