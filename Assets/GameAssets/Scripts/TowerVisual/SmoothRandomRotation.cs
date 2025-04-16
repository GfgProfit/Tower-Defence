using UnityEngine;

public class SmoothRandomRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _axisChangeSpeed = 1f;

    private Vector3 _currentAxis;
    private Vector3 _targetAxis;

    private void Start()
    {
        _currentAxis = Random.onUnitSphere;
        _targetAxis = Random.onUnitSphere;
    }

    private void Update()
    {
        _currentAxis = Vector3.Lerp(_currentAxis, _targetAxis, _axisChangeSpeed * Time.deltaTime);

        transform.Rotate(_currentAxis, _rotationSpeed * Time.deltaTime);

        if (Random.value < 0.01f)
        {
            _targetAxis = Random.onUnitSphere;
        }
    }
}