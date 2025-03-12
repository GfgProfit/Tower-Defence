using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    private float _speed = 0;
    private float _damage = 0;

    public void Setup(Transform enemy, float damage, float speed)
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

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) < 0.05f)
        {
            //target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
