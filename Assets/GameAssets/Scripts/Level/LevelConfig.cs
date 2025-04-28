using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private string _levelName;
    [SerializeField] private string _sceneName;
    [SerializeField] private Sprite _previewImage;

    public string LevelName => _levelName;
    public string SceneName => _sceneName;
    public Sprite PreviewImage => _previewImage;
}