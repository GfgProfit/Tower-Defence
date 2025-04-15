using GameAssets.Global.Core;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private HealthBase _portalHealth;

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnPortalTakeDamage += _portalHealth.TakeDamage;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnPortalTakeDamage -= _portalHealth.TakeDamage;
    }
}