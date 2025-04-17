using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOrbitController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private float _verticalMinAngle = -30f;
    [SerializeField] private float _verticalMaxAngle = 60f;

    [Header("Zoom (FOV) Settings")]
    [SerializeField] private float _zoomSensitivity = 10f;
    [SerializeField] private float _minFOV = 20f;
    [SerializeField] private float _maxFOV = 60f;
    [SerializeField] private float _zoomSmoothSpeed = 10f;

    private float _yaw;
    private float _pitch;
    private float _targetFOV;

    private Camera _camera;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        _camera = GetComponent<Camera>();
        _targetFOV = _camera.fieldOfView;
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _yaw += Input.GetAxis("Mouse X") * _sensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, _verticalMinAngle, _verticalMaxAngle);
        }

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -_distance);

        transform.position = _target.position + offset;
        transform.LookAt(_target);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            _targetFOV -= scroll * _zoomSensitivity;
            _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);
        }

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, Time.deltaTime * _zoomSmoothSpeed);
    }
}
