using GameAssets.Global.Core;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [SerializeField] private Enemy _me;

    [Space]
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;

    protected override void Die()
    {
        GameManager.Instance.EventBus.OnMoneyGather?.Invoke(_me.GetMoneyGathering());

        Destroy(gameObject);
    }

    protected override void DisplayHealth()
    {
        _healthHolder.gameObject.SetActive(_currentHealth < _maxHealth);
        _healthImage.fillAmount = _currentHealth / _maxHealth;
    }
}