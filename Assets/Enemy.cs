using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _healthImage;

    [Space]
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _currentHealth = 5.0f;

    private Transform[] _waypoints;
    private int _waypointIndex = 0;
    private float _maxHealth;

    private void Awake()
    {
        _maxHealth = _currentHealth;
    }

    public void SetPath(Transform[] path)
    {
        _waypoints = path;
        transform.position = _waypoints[0].position;
    }

    private void Update()
    {
        _healthImage.transform.rotation = Quaternion.LookRotation((_healthImage.transform.position - Camera.main.transform.position).normalized);

        if (_waypoints == null || _waypointIndex >= _waypoints.Length)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _waypoints[_waypointIndex].position) < 0.05f)
        {
            _waypointIndex++;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthImage.fillAmount = _currentHealth / _maxHealth;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}