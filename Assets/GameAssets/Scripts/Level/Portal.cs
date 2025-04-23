using GameAssets.Global.Core;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private PortalHealth _portalHealth;

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnPortalTakeDamage += _portalHealth.TakeDamageExtension;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnPortalTakeDamage -= _portalHealth.TakeDamageExtension;
    }
}