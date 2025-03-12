using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private Transform _towerHead;

    [Space]
    [SerializeField] private float _rotationSpeed = 5.0f;
    [SerializeField] private float _detectionRange = 3.0f;
    [SerializeField] private float _fireRate = 150.0f; //Выстрелов в минуту
    [SerializeField] protected Transform _firePoint;

    [Space]
    [SerializeField] private Color _gizmoColor = Color.white;

    protected Enemy _currentTarget;
    private float _fireCooldown = 0f;

    private void Update()
    {
        if (_currentTarget == null || !IsTargetValid(_currentTarget))
        {
            _currentTarget = FindFirstEnemy();
        }

        if (_currentTarget != null)
        {
            RotateTowards(_currentTarget.transform);

            Shoot();
        }
    }

    private bool IsTargetValid(Enemy target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        return distance <= _detectionRange;
    }

    private Enemy FindFirstEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                return enemy;
            }
        }

        return null;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        targetRotation.x = 0;
        targetRotation.z = 0;

        _towerHead.localRotation = Quaternion.Slerp(_towerHead.localRotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _detectionRange);
    }

    private void Shoot()
    {
        if (_fireCooldown <= 0f)
        {
            Fire();

            _fireCooldown = 1f / (_fireRate / 60);
        }

        _fireCooldown -= Time.deltaTime;
    }

    protected abstract void Fire();
}