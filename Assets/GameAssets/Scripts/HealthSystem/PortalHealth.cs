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

        _healthText.rectTransform.DOPunchScale(Vector3.one * -0.2f, 0.2f, 10, 1);

        if (_portalTransform == null)
        {
            return;
        }

        _portalTransform.DOPunchScale(Vector3.one * -0.2f, 0.2f, 10, 1);
    }
}