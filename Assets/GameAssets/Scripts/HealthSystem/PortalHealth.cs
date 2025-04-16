using DG.Tweening;
using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class PortalHealth : HealthBase
{
    [SerializeField] private Transform _portalTransform;
    [SerializeField] private TMP_Text _healthText;

    protected override void Die()
    {
        GameController.Instance.EventBus.RaiseGameOver();

        Debug.Log("Game Over!");
    }

    protected override void DisplayHealth()
    {
        if (_healthText == null)
        {
            return;
        }

        _healthText.text = $"<color=#FF807A>HP:</color> {_currentHealth}";
    }

    protected override void ScaleAnimation()
    {
        if (_currentHealth <= 0)
        {
            return;
        }

        DOTween.Sequence()
            .Append(_healthText.rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack))
            .Append(_healthText.rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));

        if (_portalTransform == null)
        {
            return;
        }

        DOTween.Sequence()
            .Append(_portalTransform.DOScale(0.9f, 0.125f).SetEase(Ease.OutBack))
            .Append(_portalTransform.DOScale(1.0f, 0.125f).SetEase(Ease.OutBack));
    }
}