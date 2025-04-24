using UnityEditor;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] protected Transform _towerHead;
    [SerializeField] private bool _rotateToEnemy = true;
    [SerializeField] protected float _rotationSpeed = 5f;
    [SerializeField] private ShopItemConfig _shopItemConfig;

    [Header("Vision Settings")]
    [SerializeField] protected float _visionRange = 5f;
    [SerializeField, Range(0, 180)] protected float _fieldOfViewAngle = 10f;

    [Header("Upgrade Settings")]
    [SerializeField] private int _baseUpgradePrice;
    [SerializeField] private float _upgradePriceMultiplier = 1.5f;
    [SerializeField] private int _maxUpgrades = 15;

    [Header("Automatic Level Settings")]
    [SerializeField] private int _baseExpToLevel = 100;
    [SerializeField] private float _autoLevelMultiplier = 1.02f;

    public ShopItemConfig ShopItemConfig => _shopItemConfig;
    public int UpgradeLevel => _upgradeLevel;
    public int MaxUpgrades => _maxUpgrades;
    public int AutomaticLevel { get; private set; } = 1;
    public int CurrentExpirience { get; private set; } = 0;
    public int ExpirienceToNextLevel { get; private set; } = 0;

    public int TotalInvested => _totalInvested;
    public float TotalDamageDeal { get; protected set; } = 0.0f;
    public int TotalEnemyKilled { get; protected set; } = 0;

    protected Transform _currentTarget;
    protected bool CanAttack { get; private set; }

    private int _upgradeLevel = 0;
    private int _totalInvested;

    protected virtual void Awake()
    {
        ExpirienceToNextLevel = _baseExpToLevel;
    }

    protected virtual void Update()
    {
        UpdateTarget();

        if (_currentTarget == null)
        {
            CanAttack = false;
            return;
        }

        if (_rotateToEnemy)
        {
            RotateTowards(_currentTarget);
        }

        CanAttack = IsInVisionCone(_currentTarget);
    }

    protected virtual void UpgradeByAutoLevel() { }

    private void UpdateTarget()
    {
        if (_currentTarget == null || !IsTargetAlive(_currentTarget))
        {
            _currentTarget = FindClosestEnemyInRange();
        }
    }

    public void AddInvestment(int amount)
    {
        _totalInvested += amount;
    }

    private bool IsTargetAlive(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(_towerHead.position, target.position);

        return distance <= _visionRange && target.GetComponent<EnemyBase>() != null;
    }

    private Transform FindClosestEnemyInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(_towerHead.position, _visionRange);
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent<EnemyBase>(out var enemy))
            {
                continue;
            }

            Vector3 direction = (enemy.transform.position - _towerHead.position).normalized;
            float distance = Vector3.Distance(_towerHead.position, enemy.transform.position);

            if (Physics.Raycast(_towerHead.position, direction, out RaycastHit hit, _visionRange))
            {
                if (hit.transform != enemy.transform)
                {
                    continue;
                }

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy.transform;
                }
            }
        }

        return closest;
    }

    private bool IsInVisionCone(Transform target)
    {
        Vector3 directionToTarget = (target.position - _towerHead.position).normalized;
        float angle = Vector3.Angle(_towerHead.forward, directionToTarget);

        return angle <= _fieldOfViewAngle;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = target.position - _towerHead.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _towerHead.rotation = Quaternion.Slerp(_towerHead.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public float GetVisionRange() => _visionRange;

    public int GetNextUpgradePrice()
    {
        return Mathf.RoundToInt(_baseUpgradePrice * Mathf.Pow(_upgradePriceMultiplier, _upgradeLevel));
    }

    public virtual void Upgrade()
    {
        if (_upgradeLevel < MaxUpgrades)
        {
            _upgradeLevel++;
        }
    }

    public void NotifyKill()
    {
        TotalEnemyKilled++;
    }

    public void AddExpirience(int exp)
    {
        CurrentExpirience += exp;

        if (CurrentExpirience >= ExpirienceToNextLevel)
        {
            int a = CurrentExpirience - ExpirienceToNextLevel;
            CurrentExpirience = a;
            AutomaticLevel++;

            UpgradeByAutoLevel();
        }

        ExpirienceToNextLevel = CalcuteExpToNextLevel();
    }

    public int CalcuteExpToNextLevel()
    {
        return Mathf.RoundToInt(_baseExpToLevel * Mathf.Pow(_autoLevelMultiplier, AutomaticLevel));
    }

    private void OnDrawGizmosSelected()
    {
        if (_towerHead != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_towerHead.position, _visionRange);

            Vector3 origin = _towerHead.position;
            Vector3 forward = _towerHead.forward;

            Quaternion leftRotation = Quaternion.Euler(0, -_fieldOfViewAngle, 0);
            Quaternion rightRotation = Quaternion.Euler(0, _fieldOfViewAngle, 0);

            Vector3 leftDir = leftRotation * forward;
            Vector3 rightDir = rightRotation * forward;

            Gizmos.color = new Color(231f / 255f, 74f / 255f, 0, 0.2f);
            Gizmos.DrawRay(origin, leftDir * _visionRange);
            Gizmos.DrawRay(origin, rightDir * _visionRange);

#if UNITY_EDITOR
            Handles.color = new Color(231f / 255f, 131f / 255f, 0, 0.2f);
            Handles.DrawSolidArc(
                origin,
                Vector3.up,
                leftDir,
                _fieldOfViewAngle * 2f,
                _visionRange
            );
#endif
        }
    }
}