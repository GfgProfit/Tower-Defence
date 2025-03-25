using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy _target;
    private float _speed = 0;
    private float _damage = 0;

    public void Construct(Enemy enemy, float damage, float speed)
    {
        _target = enemy;
        _damage = damage;
        _speed = speed;

        StartCoroutine(MoveToEnemy());
    }

    private IEnumerator MoveToEnemy()
    {
        while (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.transform.position) < 0.05f)
            {
                _target.GetHealthComponent().TakeDamage(_damage);
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
