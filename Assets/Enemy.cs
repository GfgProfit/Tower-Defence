using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;

    private Transform[] _waypoints;
    private int _waypointIndex = 0;

    public void SetPath(Transform[] path)
    {
        _waypoints = path;
        transform.position = _waypoints[0].position;
    }

    private void Update()
    {
        if (_waypoints == null || _waypointIndex >= _waypoints.Length)
        {
            Destroy(gameObject);
            
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _waypoints[_waypointIndex].position) < 0.05f)
        {
            _waypointIndex++;
        }
    }
}
