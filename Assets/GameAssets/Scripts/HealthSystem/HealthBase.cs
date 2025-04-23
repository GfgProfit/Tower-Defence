using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5;

    public float MaxHealth { get; private set; }

    protected bool _isDead;

    protected abstract void DisplayHealth();
    protected abstract void Die();

    private void Awake()
    {
        DisplayHealth();
    }

    public float TakeDamage(float damage)
    {
        if (_isDead)
        {
            return 0f;
        }

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
}