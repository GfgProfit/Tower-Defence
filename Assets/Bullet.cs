using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy _target;
    private float _speed = 0;
    private float _damage = 0;

    public void Setup(Enemy enemy, float damage, float speed)
    {
        _target = enemy;
        _damage = damage;
        _speed = speed;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject); return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.transform.position) < 0.05f)
        {
            _target.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}
