using UnityEngine;

public interface IDamageable
{
    int Health { set; get; }
    bool Targetable { set; get; }

    void OnHit(int damage, Vector3 knockback, bool takesKnockBack);
    void OnHit(int damage, GameObject hit, bool limbDamage);
    void RemoveCharacter();
}
