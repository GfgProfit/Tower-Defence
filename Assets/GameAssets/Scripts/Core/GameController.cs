using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets.Global.Core
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        private static Bootstrapper _bootstrapper;

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
            _bootstrapper = new(1);

            EventBus = new();

            DOTween.SetTweensCapacity(500, 200);
        }
    }

    public class Bootstrapper
    {
        public Bootstrapper(int sceneId)
        {
            SceneManager.LoadScene(sceneId);

            //etc services
        }
    }
}