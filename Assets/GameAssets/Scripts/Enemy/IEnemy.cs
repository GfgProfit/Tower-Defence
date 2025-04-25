using System;
using UnityEngine;

public interface IEnemy
{
    EnemyType Type { get; }
    HealthBase HealthComponent { get; }
    Transform Transform { get; }

    int MoneyGathering { get; }
    
    void Initialize(int money, float health);
    void InitializeSpeed(float speed);
    void SetPath(Transform[] path);
    void Stun(float duration);
    void ApplySlow(float multiplier, float duration);

    Action OnDeath { get; set; }
}