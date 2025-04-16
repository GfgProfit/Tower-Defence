using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5;

    public float MaxHealth { get; private set; }

    protected abstract void DisplayHealth();
    protected abstract void Die();
    protected virtual void ScaleAnimation() { }

    private void Awake()
    {
        MaxHealth = _currentHealth;
        
        DisplayHealth();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        DisplayHealth();
        ScaleAnimation();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > MaxHealth)
        {
            _currentHealth = MaxHealth;
        }

        DisplayHealth();
        ScaleAnimation();
    }
}