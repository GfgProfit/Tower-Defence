using GameAssets.Global.Core;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

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

    protected override void ScaleAnimation()
    {
        if (_me != null)
        {
            DOTween.Sequence()
                .Append(_me.transform.DOScale(0.35f, 0.1f).SetEase(Ease.OutBack))
                .Append(_me.transform.DOScale(0.45f, 0.1f).SetEase(Ease.OutBack));
        }
    }
}