using System;
using UnityEngine;

public interface IEnemy
{
    HealthBase HealthComponent { get; }
    
    int MoneyGathering { get; }
    
    void Initialize(int money, float health);
    void InitializeSpeed(float speed);
    void SetPath(Transform[] path);
    void Stun(float duration);
    void ApplySlow(float multiplier, float duration);
    void Activate();
    void Deactivate();

    Action OnDeath { get; set; }
}