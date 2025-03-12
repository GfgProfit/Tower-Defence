using UnityEngine;

public class TurretTower : Tower
{
    [SerializeField] private Bullet _bulletPrefab;

    [Space]
    [SerializeField] private float _damage = 1.0f;
    [SerializeField] private float _speed = 10.0f;

    protected override void Fire()
    {
        Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        bullet.Setup(_currentTarget.transform, _damage, _speed);
    }
}