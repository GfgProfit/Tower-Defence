using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Y;
    [SerializeField] private float _rotationSpeed = 100f;

    private void Update()
    {
        Vector3 axis = GetAxis();
        transform.Rotate(axis * _rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetAxis()
    {
        return _rotationAxis switch
        {
            RotationAxis.X => Vector3.right,
            RotationAxis.Y => Vector3.up,
            RotationAxis.Z => Vector3.forward,
            _ => Vector3.zero
        };
    }
}