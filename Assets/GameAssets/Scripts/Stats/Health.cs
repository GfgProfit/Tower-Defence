using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5.0f;

    protected float _maxHealth = 0.0f;

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
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            Die();
        }

        DisplayHealth();
        ScaleAnimation();
    }
}