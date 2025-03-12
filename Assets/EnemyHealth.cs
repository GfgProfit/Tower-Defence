using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private Transform _healthHolder;

    private void Update() => _healthHolder.rotation = LookAtHealthImage();

    private Quaternion LookAtHealthImage() => Quaternion.LookRotation((_healthHolder.position - Camera.main.transform.position).normalized);

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void DisplayHealth()
    {
        _healthImage.fillAmount = _currentHealth / _maxHealth;
    }
}