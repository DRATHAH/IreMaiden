using UnityEngine;

public interface IDamageable
{
    int Health { set; get; }
    bool Targetable { set; get; }

    void Recoil(float knockback, Vector3 position, float radius, float upwardsMod, float stunTime);
    void OnHit(int damage, GameObject hit, bool limbDamage);
    void RemoveCharacter();
}
