using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5;

    public float MaxHealth { get; private set; }
    public bool IsDead => _isDead;

    protected bool _isDead;
    protected TowerBase _lastDamageSource;

    protected abstract void DisplayHealth();
    protected abstract void Die();

    protected virtual void Awake()
    {
        DisplayHealth();
    }

    public float TakeDamage(float damage, TowerBase source)
    {
        if (_isDead)
        {
            return 0f;
        }

        _lastDamageSource = source;

        float actualDamage = Mathf.Min(_currentHealth, damage);
        _currentHealth -= actualDamage;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Die();
        }

        DisplayHealth();

        return actualDamage;
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
        _currentHealth = MaxHealth;

        DisplayHealth();
    }

    public void ResetHealth()
    {
        _currentHealth = MaxHealth;

        DisplayHealth();
    }

    public void Dead(bool value) => _isDead = value;
}