using System;
using GameAssets.Global.Core;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyEffectsHandler))]
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private int _damage = 1;

    public Action OnDeath { get; set; }
    public HealthBase HealthComponent { get; private set; }
    public int MoneyGathering { get; private set; }
    public Transform Transform { get; private set; }

    public EnemyType Type => _type;

    protected EnemyMovement _movement;
    protected EnemyEffectsHandler _effects;
 
    protected virtual void Awake()
    {
        Transform = transform;

        HealthComponent = GetComponent<EnemyHealth>();
        _movement = GetComponent<EnemyMovement>();
        _effects = GetComponent<EnemyEffectsHandler>();
    }

    protected virtual void OnEnable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver += OnGameOver;
        _movement.OnPathComplete += ReachEnd;
    }

    protected virtual void OnDisable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver -= OnGameOver;
        _movement.OnPathComplete -= ReachEnd;
    }

    protected virtual void Update()
    {
        if (_effects.IsStunned)
        {
            return;
        }

        _movement.Move(transform);
    }

    public virtual void Initialize(int money, float health)
    {
        MoneyGathering = money;
        HealthComponent.SetMaxHealth(health);
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
        Bootstrapper.Instance.EventBus.RaisePortalTakeDamage(_damage);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void Stun(float duration) => _effects.Stun(duration);
    public void ApplySlow(float multiplier, float duration) => _effects.ApplySlow(multiplier, duration);
}
