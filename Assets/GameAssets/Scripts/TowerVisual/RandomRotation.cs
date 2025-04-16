using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50f;

    private Vector3 _randomAxis;

    private void Start()
    {
        _randomAxis = Random.onUnitSphere;
    }

    private void Update()
    {
        transform.Rotate(_randomAxis, _rotationSpeed * Time.deltaTime);
    }
}