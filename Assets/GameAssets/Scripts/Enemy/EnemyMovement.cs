using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _reachThreshold = 0.05f;
    private Transform[] _waypoints;
    private int _waypointIndex;
    private float _speed;

    public event Action OnPathComplete;
    public float CurrentSpeed { get; private set; }

    public void InitializePath(Transform[] path)
    {
        _waypoints = path;
        _waypointIndex = 0;
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
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, target.position, CurrentSpeed * Time.deltaTime);

        RotateTowards(enemyTransform, target);

        if (Vector3.Distance(enemyTransform.position, target.position) < _reachThreshold)
        {
            _waypointIndex++;
            if (_waypointIndex >= _waypoints.Length)
            {
                OnPathComplete?.Invoke();
            }
        }
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
}