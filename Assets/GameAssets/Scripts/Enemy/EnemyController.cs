using System.Collections;
using GameAssets.Global.Core;
using UnityEngine;

public class EnemyController : MonoBehaviour, IStunnable
{
    [SerializeField] private HealthBase _enemyHealth;
    [SerializeField] private RectTransform _canvasHolder;

    [Space]
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private int _damage = 1;

    [Space]
    [SerializeField] private int _moneyGathering = 10;

    private Transform[] _waypoints;
    private int _waypointIndex = 0;
    private Transform _mainCameraTransform;
    private const float REACH_THRESHOLD = 0.05f;
    private Coroutine _slowCoroutine;
    private float _currentSpeed;
    private bool _isStunned;
    private float _stunTimer;

    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
        _currentSpeed = _speed;
    }

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnGameOver -= GameOver;
    }

    private void Update()
    {
        _canvasHolder.transform.forward = _mainCameraTransform.forward;

        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
            }
            return;
        }

        if (_waypoints == null || _waypointIndex >= _waypoints.Length)
        {
            return;
        }

        MoveToPoint();
    }

    public void SetPath(Transform[] path)
    {
        if (path == null || path.Length == 0)
        {
            Debug.LogWarning("Enemy path is null or empty.");
            return;
        }

        _waypoints = path;
        _waypointIndex = 0;
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
        }

        _slowCoroutine = StartCoroutine(SlowDownCoroutine(multiplier, duration));
    }

    private IEnumerator SlowDownCoroutine(float multiplier, float duration)
    {
        _currentSpeed = _speed / multiplier;
        yield return new WaitForSeconds(duration);
        _currentSpeed = _speed;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
    }

    private void MoveToPoint()
    {
        if (_waypoints != null && _waypointIndex < _waypoints.Length && _waypoints[_waypointIndex] != null && !_isStunned)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _currentSpeed * Time.deltaTime);
            RotateTowards(_waypoints[_waypointIndex]);
        }

        float distance = Vector3.Distance(transform.position, _waypoints[_waypointIndex].position);

        if (distance < REACH_THRESHOLD)
        {
            _waypointIndex++;
        }

        if (_waypointIndex >= _waypoints.Length)
        {
            ReachEnd();
            return;
        }
    }

    private void GameOver()
    {
        Destroy(gameObject);
    }

    private void ReachEnd()
    {
        GameController.Instance.EventBus.RaisePortalTakeDamage(_damage);

        Destroy(gameObject);
    }

    public HealthBase GetHealthComponent() => _enemyHealth;
    public int GetMoneyGathering() => _moneyGathering;

    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
    }
}