using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5;

    protected float _maxHealth = 0;

    protected abstract void DisplayHealth();
    protected abstract void Die();
    protected virtual void ScaleAnimation() { }

    private void Awake()
    {
        _maxHealth = _currentHealth;
        
        DisplayHealth();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        DisplayHealth();
        ScaleAnimation();
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        DisplayHealth();
        ScaleAnimation();
    }
}