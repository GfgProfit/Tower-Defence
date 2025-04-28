using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets.Global.Core
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        public static Bootstrapper Instance => _instance;
        public static IServiceLocator ServiceLocator => _serviceLocator;

        private static Bootstrapper _instance;
        private static IServiceLocator _serviceLocator;

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

            _serviceLocator = new ServiceLocator();

            RegisterServices();
        }

        private void RegisterServices()
        {
            _serviceLocator.Register<IEnemyFactoryService>(RegisterEnemyFactory());
        }

        private static EnemyFactoryService RegisterEnemyFactory()
        {
            EnemyFactoryService factoryService = new();

            TankEnemyFactory tankEnemyFactory = new(Resources.Load<TankEnemy>("Prefabs/Enemies/TankEnemy"));
            FastEnemyFactory fastEnemyFactory = new(Resources.Load<FastEnemy>("Prefabs/Enemies/FastEnemy"));
            WeakEnemyFactory weakEnemyFactory = new(Resources.Load<WeakEnemy>("Prefabs/Enemies/WeakEnemy"));

            factoryService.RegisterFactory(EnemyType.Tank, tankEnemyFactory);
            factoryService.RegisterFactory(EnemyType.Fast, fastEnemyFactory);
            factoryService.RegisterFactory(EnemyType.Weak, weakEnemyFactory);

            return factoryService;
        }

        public static void LoadScene(int id) => SceneManager.LoadScene(id);
        public static void LoadScene(string name) => SceneManager.LoadScene(name);
        public static void LoadSceneAsync(int id) => SceneManager.LoadSceneAsync(id);
        public static void LoadSceneAsync(string name) => SceneManager.LoadSceneAsync(name);
    }
}