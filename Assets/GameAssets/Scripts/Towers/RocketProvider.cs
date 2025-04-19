using UnityEngine;

public class RocketProvider : MonoBehaviour, IRocketProvider
{
    private IRocket[] _rockets;

    public IRocket[] GetAvailableRockets()
    {
        _rockets = GetComponentsInChildren<IRocket>(true);
        return _rockets;
    }
}