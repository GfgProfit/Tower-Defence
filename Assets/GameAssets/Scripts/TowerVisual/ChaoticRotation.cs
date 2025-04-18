using UnityEngine;

public class ChaoticRotation : MonoBehaviour
{
    [SerializeField] private RotationAxis _rotationAxis;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _rotationMultiplier = 1;

    private void Update()
    {
        Vector3 axis = GetRandomizedAxis();
        transform.Rotate(axis, _rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomizedAxis()
    {
        return _rotationAxis switch
        {
            RotationAxis.All => new Vector3(Random.Range(-_rotationMultiplier, _rotationMultiplier), Random.Range(-_rotationMultiplier, _rotationMultiplier), Random.Range(-_rotationMultiplier, _rotationMultiplier)).normalized,
            RotationAxis.X => new Vector3(Random.Range(-_rotationMultiplier, _rotationMultiplier), 0f, 0f).normalized,
            RotationAxis.Y => new Vector3(0f, Random.Range(-_rotationMultiplier, _rotationMultiplier), 0f).normalized,
            RotationAxis.Z => new Vector3(0f, 0f, Random.Range(-_rotationMultiplier, _rotationMultiplier)).normalized,
            _ => Vector3.zero
        };
    }
}