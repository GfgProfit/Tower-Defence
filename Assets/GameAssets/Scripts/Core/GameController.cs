using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets.Global.Core
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        public static GameController Instance => _instance;

        [SerializeField] private WeakEnemy _weakEnemyPrefab;
        [SerializeField] private FastEnemy _fastEnemyPrefab;
        [SerializeField] private TankEnemy _tankEnemyPrefab;

        private static GameController _instance;

        public EventBus EventBus { get; private set; }

        private void Awake()
        {
            Singleton();

            Initialize();
        }

        private void Singleton()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);

                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Initialize()
        {
            SceneManager.LoadScene(1);

            DOTween.SetTweensCapacity(500, 200);

            EventBus = new();
        }
    }
}