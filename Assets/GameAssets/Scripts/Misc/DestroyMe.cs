using System.Collections;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 1.0f;
    [SerializeField] private float _delayBeforeDestroy = 1.0f;

    private void Awake() => StartCoroutine(Destroy());

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_delayBeforeDestroy);

        Destroy(gameObject, _destroyDelay);
    }
}