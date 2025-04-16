using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private Transform _towerHead;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _fireRate = 60f;

    [Header("Vision Settings")]
    [SerializeField] private float _visionRange = 10f;
    [SerializeField] private float _fieldOfViewAngle = 20f;

    private float _fireCooldown;
    protected Transform _currentTarget;

    protected IAttackBehavior _attackBehavior;

    protected abstract IAttackBehavior CreateAttackBehavior();
    protected virtual void StartEffect() { }
    protected virtual void StopEffect() { }
    protected virtual void ApplyEffect() 
    {
        if (_currentTarget == null)
        {
            return;
        }
    }

    private void Start()
    {
        _attackBehavior = CreateAttackBehavior();
    }

    private void Shoot(Transform target)
    {
        _attackBehavior?.Attack(target);

        ApplyEffect();
    }

    private void Update()
    {
        UpdateTarget();

        if (_currentTarget == null)
        {
            StopEffect();
            return;
        }

        RotateTowards(_currentTarget);

        if (IsInVisionCone(_currentTarget))
        {
            if (_fireRate > 0)
            {
                if (_fireCooldown <= 0f)
                {
                    Shoot(_currentTarget);
                    _fireCooldown = 60f / _fireRate;
                }

                _fireCooldown -= Time.deltaTime;
            }
            else if (_fireRate <= 0f)
            {
                Shoot(_currentTarget);
            }

            StartEffect();
        }
        else
        {
            StopEffect();
        }
    }

    private void UpdateTarget()
    {
        if (_currentTarget == null || !IsTargetAlive(_currentTarget))
        {
            _currentTarget = FindClosestEnemyInRange();
        }
    }

    private bool IsTargetAlive(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(_towerHead.position, target.position);

        return distance <= _visionRange && target.GetComponent<EnemyController>() != null;
    }

    private Transform FindClosestEnemyInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(_towerHead.position, _visionRange);
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent<EnemyController>(out var enemy))
            {
                continue;
            }

            Vector3 direction = (enemy.transform.position - _towerHead.position).normalized;
            float distance = Vector3.Distance(_towerHead.position, enemy.transform.position);

            if (Physics.Raycast(_towerHead.position, direction, out RaycastHit hit, _visionRange))
            {
                if (hit.transform != enemy.transform)
                {
                    continue;
                }

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy.transform;
                }
            }
        }

        return closest;
    }

    private bool IsInVisionCone(Transform target)
    {
        Vector3 directionToTarget = (target.position - _towerHead.position).normalized;
        float angle = Vector3.Angle(_towerHead.forward, directionToTarget);

        return angle <= _fieldOfViewAngle;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = target.position - _towerHead.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _towerHead.rotation = Quaternion.Slerp(_towerHead.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (_towerHead != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_towerHead.position, _visionRange);

            Vector3 origin = _towerHead.position;
            Vector3 forward = _towerHead.forward;

            Quaternion left = Quaternion.Euler(0, -_fieldOfViewAngle, 0);
            Quaternion right = Quaternion.Euler(0, _fieldOfViewAngle, 0);

            Vector3 leftDir = left * forward;
            Vector3 rightDir = right * forward;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(origin, leftDir * _visionRange);
            Gizmos.DrawRay(origin, rightDir * _visionRange);
        }
    }
}
