using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class PortalHealth : HealthBase
{
    [SerializeField] private Transform _portalTransform;
    [SerializeField] private TMP_Text _healthText;

    protected override void Die()
    {
        _isDead = true;

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
}