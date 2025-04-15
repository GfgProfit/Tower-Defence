using UnityEngine;

public class PathPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public Transform[] GetWaypoints() => _waypoints;
}