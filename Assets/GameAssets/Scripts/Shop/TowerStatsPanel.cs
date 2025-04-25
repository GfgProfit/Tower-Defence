using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameAssets.Global.Core;
using System.Collections.Generic;

public class TowerStatsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Slider _upgradeSlider;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private TMP_Text _sellMoneyText;
    [SerializeField] private TMP_Text _upgradeMoneyText;
    [SerializeField] private TMP_Text _totalDamageDealText;
    [SerializeField] private TMP_Text _totalEnemyKilledText;
    [SerializeField] private TMP_Text _autoLevelText;
    [SerializeField] private CustomButton _sellButton;
    [SerializeField] private CustomButton _upgradeButton;

    private string _currentStatsText;
    private TowerTile _currentTile;
    private ITowerStats _currentTowerStats;
    private UpgradeButtonHoverHandler _upgradeHoverHandler;

    private void Awake()
    {
        _sellButton.OnClick.AddListener(OnSellButtonClicked);
        _upgradeButton.OnClick.AddListener(() => Bootstrapper.Instance.EventBus.RaiseUpgradeTower(_currentTile));

        _upgradeHoverHandler = _upgradeButton.GetComponent<UpgradeButtonHoverHandler>();

        _upgradeHoverHandler.OnPointerEnterEvent += ShowUpgradePreview;
        _upgradeHoverHandler.OnPointerExitEvent += RestoreStats;
    }

    private void Update()
    {
        if (_currentTile == null)
        {
            return;
        }

        DisplayTotal(_currentTile);
        RefreshStats();
    }

    public void Show(TowerTile tile)
    {
        _panel.gameObject.SetActive(true);

        _nameText.text = $"<color={Utils.ColorToHex(tile.MyTower.ShopItemConfig.NameColor)}>{tile.MyTower.ShopItemConfig.Name}</color>";
        _statsText.text = "";
        _currentStatsText = "";
        _currentTile = tile;
        _currentTowerStats = tile.MyTower as ITowerStats;
        _upgradeMoneyText.text = tile.MyTower.UpgradeLevel < tile.MyTower.MaxUpgrades ? Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.').ToString() : "MAX";
        _autoLevelText.text = $"L{tile.MyTower.AutomaticLevel}\n<size=18>{tile.MyTower.CurrentExpirience} / {tile.MyTower.ExpirienceToNextLevel} XP</size>";

        if (tile != null ? tile.MyTower : null != null && tile.MyTower.ShopItemConfig != null)
        {
            int sellPrice = Mathf.RoundToInt(tile.MyTower.TotalInvested * tile.MyTower.ShopItemConfig.SellMultiplier);

            _sellMoneyText.text = Utils.FormatNumber(sellPrice, '.').ToString();
            _sellButton.gameObject.SetActive(true);
        }

        UpdateUpgradeProgress();
    }

    private void DisplayTotal(TowerTile tile)
    {
        _totalDamageDealText.text = Utils.FormatCompactNumber(tile.MyTower.TotalDamageDeal);
        _totalEnemyKilledText.text = tile.MyTower.TotalEnemyKilled.ToString();
        _autoLevelText.text = $"L{tile.MyTower.AutomaticLevel}\n<size=18>{tile.MyTower.CurrentExpirience} / {tile.MyTower.ExpirienceToNextLevel} XP</size>";
        _levelSlider.maxValue = tile.MyTower.ExpirienceToNextLevel;
        _levelSlider.value = tile.MyTower.CurrentExpirience;
    }

    public void Hide()
    {
        _panel.gameObject.SetActive(false);
        _currentTile = null;
    }

    private void OnSellButtonClicked()
    {
        if (_currentTile != null)
        {
            Bootstrapper.Instance.EventBus.RaiseSellTower(_currentTile);
        }
    }

    private void ShowUpgradePreview()
    {
        if (_currentTowerStats == null)
            return;

        if (_currentTile.MyTower.UpgradeLevel < _currentTile.MyTower.MaxUpgrades)
        {
            List<StatData> upgradeStats = _currentTowerStats.GetStatsAfterUpgrade();

            string upgradedText = string.Empty;
            foreach (var stat in upgradeStats)
            {
                //upgradedText += $"{stat.Name} <color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>{stat.Value}</color>\n";
            }

            _statsText.text = upgradedText;
            _upgradeMoneyText.text = Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.').ToString();
        }
        else
        {
            _upgradeMoneyText.text = "MAX";
        }
    }

    private void RestoreStats()
    {
        _statsText.text = _currentStatsText;
    }

    public void RefreshStats()
    {
        if (_currentTowerStats == null)
            return;

        List<StatData> statDatas = _currentTowerStats.GetStats();

        string stats = string.Empty;
        foreach (var statData in statDatas)
        {
            //stats += $"{statData.Name} <color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>{statData.Value}</color>\n";
        }

        _currentStatsText = stats;
        _statsText.text = stats;

        if (_upgradeHoverHandler != null && _upgradeHoverHandler.IsHovered)
        {
            _upgradeMoneyText.text = Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.').ToString();
            ShowUpgradePreview();
        }
    }

    private void UpdateUpgradeProgress()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        _upgradeSlider.maxValue = _currentTile.MyTower.MaxUpgrades;
        _upgradeSlider.value = _currentTile.MyTower.UpgradeLevel;
    }
}