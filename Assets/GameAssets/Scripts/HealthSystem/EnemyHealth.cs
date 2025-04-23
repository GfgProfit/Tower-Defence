using GameAssets.Global.Core;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [SerializeField] private EnemyController _me;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;

    protected override void Die()
    {
        _isDead = true;

        GameController.Instance.EventBus.RaiseMoneyGather(_me.MoneyGathering);

        _me.OnDeath?.Invoke();

        Destroy(gameObject);
    }

    protected override void DisplayHealth()
    {
        if (_healthImage == null || _healthHolder == null)
        {
            return;
        }

        _healthImage.fillAmount = _currentHealth / MaxHealth;
        _healthImage.color = Color.Lerp(Color.red, Color.green, _currentHealth / MaxHealth);
        
        _healthHolder.gameObject.SetActive(_currentHealth < MaxHealth);
    }
}