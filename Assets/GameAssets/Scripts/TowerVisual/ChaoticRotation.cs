using UnityEngine;

public class ChaoticRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        Vector3 randomAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        transform.Rotate(randomAxis, rotationSpeed * Time.deltaTime);
    }
}