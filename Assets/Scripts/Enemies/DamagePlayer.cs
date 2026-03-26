using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;

    public bool hitPlayer = false;

    void OnEnable()
    {
        hitPlayer = false;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<DamageableCharacter>(out DamageableCharacter playerHP) && hitPlayer == false)
        {
            playerHP.OnHit(damageValue, other.gameObject, false);
            hitPlayer = true;
        }
    }
}
