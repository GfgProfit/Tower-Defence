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
    }
}