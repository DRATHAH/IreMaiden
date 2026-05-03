using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;
    public bool hitPlayer = false;
    public GameObject soundParticle;
    public AudioSource hitSound;

    void OnEnable()
    {
        hitPlayer = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<DamageableCharacter>(out DamageableCharacter playerHP) && hitPlayer == false)
        {
            playerHP.OnHit(damageValue, other.gameObject, false);
            GameObject sound = Instantiate(soundParticle, transform.position, Quaternion.identity);
            sound.GetComponent<SoundObject>().Initialize(hitSound);
        }
    }
}
