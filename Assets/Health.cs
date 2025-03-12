using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5.0f;

    protected float _maxHealth = 0.0f;

    protected abstract void DisplayHealth();
    protected abstract void Die();

    private void Awake() => _maxHealth = _currentHealth;

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        DisplayHealth();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }
}