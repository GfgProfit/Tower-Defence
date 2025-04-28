using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _reachThreshold = 0.05f;

    private Transform[] _waypoints;
    private int _waypointIndex;
    private float _speed;
    private Vector3 _currentOffset;

    public event Action OnPathComplete;
    public float CurrentSpeed { get; private set; }

    public void InitializePath(Transform[] path)
    {
        _waypoints = path;
        _waypointIndex = 0;
        transform.position = _waypoints[_waypointIndex].position;
    }

    public void InitializeSpeed(float speed)
    {
        _speed = speed;
        CurrentSpeed = speed;
    }

    public void Move(Transform enemyTransform)
    {
        if (_waypoints == null || _waypointIndex >= _waypoints.Length) return;

        Transform target = _waypoints[_waypointIndex];

        if (_currentOffset == Vector3.zero)
        {
            _currentOffset = GetRandomOffset();
        }

        Vector3 targetPositionWithOffset = target.position + _currentOffset;

        enemyTransform.position = Vector3.MoveTowards(
            enemyTransform.position,
            targetPositionWithOffset,
            CurrentSpeed * Time.deltaTime
        );

        RotateTowards(enemyTransform, target);

        if (Vector3.Distance(enemyTransform.position, targetPositionWithOffset) < _reachThreshold)
        {
            _waypointIndex++;
            _currentOffset = Vector3.zero;

            if (_waypointIndex >= _waypoints.Length)
            {
                OnPathComplete?.Invoke();
            }
        }
    }

    private Vector3 GetRandomOffset()
    {
        float maxOffset = 0.3f;

        return new Vector3(
            UnityEngine.Random.Range(-maxOffset, maxOffset),
            0f,
            UnityEngine.Random.Range(-maxOffset, maxOffset)
        );
    }

    private void RotateTowards(Transform enemyTransform, Transform target)
    {
        Vector3 direction = target.position - enemyTransform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void SetSpeedMultiplier(float multiplier) => CurrentSpeed = _speed / multiplier;
    public void ResetSpeed() => CurrentSpeed = _speed;
    public void ResetPath() => _waypointIndex = 0;
}