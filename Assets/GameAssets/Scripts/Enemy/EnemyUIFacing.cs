using UnityEngine;

public class EnemyUIFacing : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasHolder;

    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;

    private void Update() => _canvasHolder.forward = _cameraTransform.forward;
}