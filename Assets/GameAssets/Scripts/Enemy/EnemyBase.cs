using System;
using GameAssets.Global.Core;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyEffectsHandler))]
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    [SerializeField] private HealthBase _enemyHealth;
    [SerializeField] private int _damage = 1;

    public Action OnDeath { get; set; }

    protected EnemyMovement _movement;
    protected EnemyEffectsHandler _effects;
    protected int _moneyGathering;

    public HealthBase HealthComponent => _enemyHealth;
    public int MoneyGathering => _moneyGathering;

    protected virtual void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _effects = GetComponent<EnemyEffectsHandler>();
    }

    protected virtual void OnEnable()
    {
        GameController.Instance.EventBus.OnGameOver += OnGameOver;
        _movement.OnPathComplete += ReachEnd;
    }

    protected virtual void OnDisable()
    {
        GameController.Instance.EventBus.OnGameOver -= OnGameOver;
        _movement.OnPathComplete -= ReachEnd;
    }

    protected virtual void Update()
    {
        if (_effects.IsStunned) return;
        _movement.Move(transform);
    }

    public virtual void Initialize(int money, float health)
    {
        _moneyGathering = money;
        _enemyHealth.SetMaxHealth(health);
    }

    public virtual void SetPath(Transform[] path)
    {
        _movement.InitializePath(path);
    }

    public virtual void InitializeSpeed(float speed)
    {
        _movement.InitializeSpeed(speed);
    }

    protected virtual void OnGameOver()
    {
        Destroy(gameObject);
    }

    protected virtual void ReachEnd()
    {
        GameController.Instance.EventBus.RaisePortalTakeDamage(_damage);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void Stun(float duration) => _effects.Stun(duration);
    public void ApplySlow(float multiplier, float duration) => _effects.ApplySlow(multiplier, duration);
}
