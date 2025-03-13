using UnityEngine;

public class TurretTower : Tower
{
    [Header("Turret Tower")]
    [SerializeField] private Bullet _bulletPrefab;

    [Space]
    [SerializeField] private float _damage = 1.0f;
    [SerializeField] private float _speed = 10.0f;

    protected override void Fire()
    {
        Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        bullet.Construct(_currentTarget, _damage, _speed);
    }
}