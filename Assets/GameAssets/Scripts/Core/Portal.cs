using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Health _portalHealth;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one / 2.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                _portalHealth.TakeDamage(enemy.GetDamage());

                Destroy(enemy.gameObject);
            }
        }
    }
}