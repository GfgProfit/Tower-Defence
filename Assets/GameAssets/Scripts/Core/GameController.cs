using UnityEngine;

namespace GameAssets.Global.Core
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;

        public static GameController Instance => _instance;

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
            EventBus = new();
        }
    }
}