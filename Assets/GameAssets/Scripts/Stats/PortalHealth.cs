using DG.Tweening;
using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class PortalHealth : Health
{
    [SerializeField] private TMP_Text _healthText;

    protected override void Die()
    {
        GameManager.Instance.EventBus.OnGameOver?.Invoke();

        Debug.Log("Game Over!");
    }

    protected override void DisplayHealth()
    {
        _healthText.text = $"<color=#FF807A>HP:</color> {_currentHealth}";

        DOTween.Sequence()
            .Append(_healthText.rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack))
            .Append(_healthText.rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));
    }
}