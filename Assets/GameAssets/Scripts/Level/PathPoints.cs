using UnityEngine;

public class PathPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public Transform[] GetWaypoints() => _waypoints;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }
    }
}