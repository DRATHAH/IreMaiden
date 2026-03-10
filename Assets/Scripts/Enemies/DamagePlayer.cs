using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<DamageableCharacter>(out DamageableCharacter playerHP))
        {
            //playerHP.OnHit(damageValue);
        }
    }
}
