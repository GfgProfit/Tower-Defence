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

        TowerBase[] allTowers = FindObjectsByType<TowerBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        float totalKills = 0;
        float totalXP = 0;
        float totalDamage = 0;
        float totalScore = (totalDamage + totalKills + totalXP) / 2;

        foreach (TowerBase tower in allTowers)
        {
            totalKills += tower.TotalEnemyKilled;
            totalXP += tower.TotalExpirience;
            totalDamage += tower.TotalDamageDeal;
        }

        _statsText.text = $"Total Kills: <color=#D66E74>{totalKills}</color>\n" +
                          $"Total XP Earned: <color=#6E9FD6>{Utils.FormatCompactNumber(totalXP)}</color>\n" +
                          $"Total Damage Deal: <color=#D6A26E>{Utils.FormatCompactNumber(totalDamage)}</color>\n\n" +
                          $"Wave Survived: {_waveGenerator.CurrentWave - 1}\n" +
                          $"Total Score: {Utils.FormatCompactNumber(totalScore)}";
    }
}