using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public Transform[] GetWaypoints() => _waypoints;
}