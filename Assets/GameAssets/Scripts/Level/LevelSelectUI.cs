using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _contentRoot;
    [SerializeField] private CustomButton _levelButtonPrefab;

    private void Awake()
    {
        LevelLoader loader = new();

        foreach (LevelConfig level in loader.Levels)
        {
            CustomButton button = Instantiate(_levelButtonPrefab, _contentRoot);
            button.GetComponentInChildren<TMP_Text>().text = level.LevelName;
            button.OnClick.AddListener(() => Bootstrapper.LoadScene(level.SceneName));
        }
    }
}