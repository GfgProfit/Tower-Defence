using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new GameObject("[Game Manager]").AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public EventBus EventBus;

    private void Awake()
    {
        EventBus = new();
    }
}