using GameAssets.Global.Core;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [SerializeField] private EnemyController _me;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;

    private void Update()
    {
        if (_isDead)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    protected override void Die()
    {
        GameController.Instance.EventBus.RaiseMoneyGather(_me.GetMoneyGathering());
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
        transform.DOPunchScale(Vector3.one * 0.2f, _animationCooldown, 10, 1)
            .OnComplete(() =>
            {
                if (_isDead)
                {
                    DOTween.Kill(transform);
                    Destroy(gameObject);
                }
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}