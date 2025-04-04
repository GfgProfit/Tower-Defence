using GameAssets.Global.Core;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _enemyHealth;

    [Space]
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _damage = 1.0f;

    [Space]
    [SerializeField] private int _moneyGathering = 10;

    private Transform[] _waypoints;
    private int _waypointIndex = 0;

    private void OnEnable()
    {
        GameManager.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameManager.Instance.EventBus.OnGameOver -= GameOver;
    }

    private void Update()
    {
        if (_waypoints == null || _waypointIndex >= _waypoints.Length)
        {
            return;
        }

        MoveToPoint();
    }

    public void SetPath(Transform[] path)
    {
        _waypoints = path;
        transform.position = _waypoints[0].position;
    }

    private void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, _waypoints[_waypointIndex].position);

        if (distance < 0.05f)
        {
            _waypointIndex++;
        }
    }

    private void GameOver()
    {
        Destroy(gameObject);
    }

    public Health GetHealthComponent() => _enemyHealth;
    public float GetDamage() => _damage;
    public int GetMoneyGathering() => _moneyGathering;
}