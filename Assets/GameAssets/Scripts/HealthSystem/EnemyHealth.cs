using GameAssets.Global.Core;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;
    
    private EnemyBase _myEnemyBase;

    protected override void Awake()
    {
        base.Awake();

        _myEnemyBase = GetComponent<EnemyBase>();
    }

    protected override void Die()
    {
        _isDead = true;

        Bootstrapper.Instance.EventBus.RaiseMoneyGather(_myEnemyBase.MoneyGathering);

        if (_lastDamageSource is TowerBase tower)
        {
            tower.NotifyKill();
        }

        _myEnemyBase.OnDeath?.Invoke();

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