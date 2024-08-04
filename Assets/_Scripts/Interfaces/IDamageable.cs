using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{ 
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float DamageReduction { get; set; }
    bool IsInvulnerable { get; set; }
    bool IsAlive { get; set; }

    void TakeDamage(float amount);
    void Die();
}
