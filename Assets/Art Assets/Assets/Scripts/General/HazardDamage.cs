using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public bool teleport; //Use this to choose if teleporting or knocking back player
    public Vector3 teleportSpot; //Use this if teleporting player
    public Vector3 recoil; //Use this to determine the knockback of a hazard
    public int damage; //Use this to determine damage of hazard

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DamageableCharacter>(out DamageableCharacter damageComponent))
        {
            if (other.tag == "Player" && teleport == false)
            {
                damageComponent.Recoil(recoil, true);
            }
            else if (other.tag == "Player" && teleport == true){
                other.transform.position = teleportSpot;
            }
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<DamageableCharacter>(out DamageableCharacter damageComponent))
        {
            damageComponent.OnHit(damage, other.gameObject, false);
        }
    }
}
