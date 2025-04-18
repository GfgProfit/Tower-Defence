using UnityEditor;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] protected Transform _towerHead;
    [SerializeField] private bool _rotateToEnemy = true;
    [SerializeField] protected float _rotationSpeed = 5f;

    [Header("Vision Settings")]
    [SerializeField] protected float _visionRange = 5f;
    [SerializeField, Range(0, 180)] protected float _fieldOfViewAngle = 10f;

    protected Transform _currentTarget;
    protected bool CanAttack { get; private set; }

    protected virtual void Update()
    {
        UpdateTarget();

        if (_currentTarget == null)
        {
            CanAttack = false;
            return;
        }

        if (_rotateToEnemy)
        {
            RotateTowards(_currentTarget);
        }

        CanAttack = IsInVisionCone(_currentTarget);
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

    public float GetVisionRange() => _visionRange;

    private void OnDrawGizmosSelected()
    {
        if (_towerHead != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_towerHead.position, _visionRange);

            Vector3 origin = _towerHead.position;
            Vector3 forward = _towerHead.forward;

            Quaternion leftRotation = Quaternion.Euler(0, -_fieldOfViewAngle, 0);
            Quaternion rightRotation = Quaternion.Euler(0, _fieldOfViewAngle, 0);

            Vector3 leftDir = leftRotation * forward;
            Vector3 rightDir = rightRotation * forward;

            Gizmos.color = new Color(231f / 255f, 74f / 255f, 0, 0.2f);
            Gizmos.DrawRay(origin, leftDir * _visionRange);
            Gizmos.DrawRay(origin, rightDir * _visionRange);

#if UNITY_EDITOR
            Handles.color = new Color(231f / 255f, 131f / 255f, 0, 0.2f);
            Handles.DrawSolidArc(
                origin,
                Vector3.up,
                leftDir,
                _fieldOfViewAngle * 2f,
                _visionRange
            );
#endif
        }
    }
}