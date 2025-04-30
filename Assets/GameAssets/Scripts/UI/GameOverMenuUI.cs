using GameAssets.Global.Core;
using TMPro;
using UnityEngine;

public class GameOverMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform _gameOverScreen;
    
    [Space]
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private CustomButton _quitButton;
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private SceneTotalHandler _sceneTotalHandler;

    private void Awake()
    {
        _quitButton.OnClick.AddListener(() => Bootstrapper.LoadScene(1));
    }

    private void OnEnable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver += ShowScreen;
    }

    private void OnDisable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver -= ShowScreen;
    }

    private void ShowScreen()
    {
        Time.timeScale = 1.0f;

        _gameOverScreen.gameObject.SetActive(true);

        _statsText.text = $"Total Kills: <color=#D66E74>{_sceneTotalHandler.TotalKills}</color>\n" +
                          $"Total XP Earned: <color=#6E9FD6>{Utils.FormatCompactNumber(_sceneTotalHandler.TotalXPEarned)}</color>\n" +
                          $"Total Damage Deal: <color=#D6A26E>{Utils.FormatCompactNumber(_sceneTotalHandler.TotalDamageDeal)}</color>\n\n" +
                          $"Wave Survived: <color=#5600FF>{_waveGenerator.CurrentWave - 1}</color>\n" +
                          $"Total Score: <color=#8BFFD5>{Utils.FormatCompactNumber((_sceneTotalHandler.TotalDamageDeal + _sceneTotalHandler.TotalKills + _sceneTotalHandler.TotalXPEarned) / 2)}</color>";
    }
}