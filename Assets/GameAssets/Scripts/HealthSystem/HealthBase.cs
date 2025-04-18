using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected float _currentHealth = 5;
    [SerializeField] protected float _animationCooldown = 0.2f;

    public float MaxHealth { get; private set; }

    private float _lastAnimationTime;
    protected bool _isDead;

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
        if (damage <= 0f || _isDead)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Die();
            _isDead = true;
        }

        DisplayHealth();

        if (Time.time - _lastAnimationTime >= _animationCooldown)
        {
            ScaleAnimation();
            _lastAnimationTime = Time.time;
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > MaxHealth)
        {
            _currentHealth = MaxHealth;
        }

        DisplayHealth();
    }
}