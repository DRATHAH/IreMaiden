using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHP))
        {
            playerHP.TakeDamage(damageValue);
        }
    }
}
