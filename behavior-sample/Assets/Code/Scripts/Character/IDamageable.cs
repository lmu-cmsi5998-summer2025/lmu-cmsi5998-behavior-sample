using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(DamageInfo amount);
    void HealAmount(float amount);
    int health { get; }
    bool isAlive { get; }
}

public struct DamageInfo
{
    public int damage;
    public int damageMultiplier;
    public GameObject origin;
    public bool ignoreInvulnerability;
}