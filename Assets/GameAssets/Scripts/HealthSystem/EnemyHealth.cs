using GameAssets.Global.Core;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [SerializeField] private EnemyController _me;

    [Space]
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;

    protected override void Die()
    {
        GameController.Instance.EventBus.RaiseMoneyGather(_me.GetMoneyGathering());

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