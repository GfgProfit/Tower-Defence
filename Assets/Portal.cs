using TMPro;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private int _portalHealth = 10;
    [SerializeField] private TMP_Text _healthText;

    private void Awake()
    {
        _healthText.text = $"{_portalHealth}";
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one / 2.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                _portalHealth--;

                if (_portalHealth <= 0)
                {
                    _portalHealth = 0;
                }

                _healthText.text = $"{_portalHealth}";

                Destroy(enemy.gameObject);
            }
        }
    }
}