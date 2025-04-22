using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameAssets.Global.Core;
using System.Collections.Generic;
using DG.Tweening;

public class TowerStatsPanel : MonoBehaviour
{
    [SerializeField] private Image[] _upgradeProgressImages;

    [Space]
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private TMP_Text _sellMoneyText;
    [SerializeField] private TMP_Text _upgradeMoneyText;
    [SerializeField] private CustomButton _sellButton;
    [SerializeField] private CustomButton _upgradeButton;
    [SerializeField] private UpgradeButtonHoverHandler _upgradeHoverHandler;
    private string _currentStatsText;
    private TowerTile _currentTile;
    private ITowerStats _currentTowerStats;

    private void Awake()
    {
        _sellButton.OnClick.AddListener(OnSellButtonClicked);
        _upgradeButton.OnClick.AddListener(() => GameController.Instance.EventBus.RaiseUpgradeTower(_currentTile));

        _upgradeHoverHandler.OnPointerEnterEvent += ShowUpgradePreview;
        _upgradeHoverHandler.OnPointerExitEvent += RestoreStats;
    }

    public void Show(Sprite icon, string towerName, string stats, TowerTile tile)
    {
        _panel.gameObject.SetActive(true);

        _iconImage.sprite = icon;
        _nameText.text = towerName;
        _statsText.text = stats;
        _currentTile = tile;
        _currentTowerStats = tile?.MyTower as ITowerStats;
        _currentStatsText = stats;

        if (tile != null ? tile.MyTower : null != null && tile.MyTower.ShopItemConfig != null)
        {
            int sellPrice = Mathf.RoundToInt(tile.MyTower.TotalInvested * tile.MyTower.ShopItemConfig.SellMultiplier);

            _sellMoneyText.text = $"<color=#FFC87F>$:</color> {Utils.FormatNumber(sellPrice, '.')}";
            _sellButton.gameObject.SetActive(true);
        }
        else
        {
            _sellMoneyText.text = "";
            _sellButton.gameObject.SetActive(false);
        }

        UpdateUpgradeProgress();
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
            GameController.Instance.EventBus.RaiseSellTower(_currentTile);
        }
    }

    private void ShowUpgradePreview()
    {
        if (_currentTowerStats == null)
            return;

        if (_currentTile.MyTower.UpgradeLevel < 15)
        {
            List<StatData> upgradeStats = _currentTowerStats.GetStatsAfterUpgrade();

            string upgradedText = string.Empty;
            foreach (var stat in upgradeStats)
            {
                upgradedText += $"{stat.Name} <color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>{stat.Value}</color>\n";
            }

            _statsText.text = upgradedText;
            _upgradeMoneyText.text = $"<color=#FFC87F>$:</color> {Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.')}";
        }
        else
        {
            _upgradeMoneyText.text = "MAX";
        }
    }

    private void RestoreStats()
    {
        _statsText.text = _currentStatsText;
        _upgradeMoneyText.text = string.Empty;
    }

    public void RefreshStats()
    {
        if (_currentTowerStats == null)
            return;

        List<StatData> statDatas = _currentTowerStats.GetStats();

        string stats = string.Empty;
        foreach (var statData in statDatas)
        {
            stats += $"{statData.Name} <color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>{statData.Value}</color>\n";
        }

        _currentStatsText = stats;
        _statsText.text = stats;

        if (_upgradeHoverHandler != null && _upgradeHoverHandler.IsHovered)
        {
            _upgradeMoneyText.text = $"<color=#FFC87F>$:</color> {Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.')}";
            ShowUpgradePreview();
        }
    }

    private void UpdateUpgradeProgress()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        int upgradeLevel = _currentTile.MyTower.UpgradeLevel;

        for (int i = 0; i < _upgradeProgressImages.Length; i++)
        {
            if (i < upgradeLevel)
                _upgradeProgressImages[i].color = Color.white;
            else
                _upgradeProgressImages[i].color = Color.gray;
        }
    }
}