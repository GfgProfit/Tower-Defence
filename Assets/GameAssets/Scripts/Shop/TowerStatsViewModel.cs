using UniRx;
using System.Collections.Generic;
using GameAssets.Global.Core;
using UnityEngine;

public class TowerStatsViewModel
{
    public ReactiveProperty<string> TowerName { get; } = new();
    public ReactiveProperty<string> StatsText { get; } = new();
    public ReactiveProperty<string> SellPrice { get; } = new();
    public ReactiveProperty<string> UpgradePrice { get; } = new();
    public ReactiveProperty<string> TotalDamage { get; } = new();
    public ReactiveProperty<string> TotalEnemiesKilled { get; } = new();
    public ReactiveProperty<string> TotalXPEarned { get; } = new();
    public ReactiveProperty<string> AutoLevelText { get; } = new();
    public ReactiveProperty<string> CurrentLevelText { get; } = new();

    public ReactiveProperty<float> UpgradeSliderValue { get; } = new();
    public ReactiveProperty<float> UpgradeSliderMax { get; } = new();
    public ReactiveProperty<float> LevelSliderValue { get; } = new();
    public ReactiveProperty<float> LevelSliderMax { get; } = new();

    public ReactiveProperty<string> UpgradeButtonText { get; } = new("UPGRADE");
    public ReactiveProperty<Color> UpgradeButtonColor { get; } = new(Color.white);

    public ReactiveCommand SellCommand { get; } = new();
    public ReactiveCommand UpgradeCommand { get; } = new();

    private TowerTile _currentTile;
    private ITowerStats _currentTowerStats;
    private bool _awaitingUpgradeConfirmation;

    public void Show(TowerTile tile)
    {
        if (tile == null || tile.MyTower == null)
            return;

        _currentTile = tile;
        _currentTowerStats = tile.MyTower as ITowerStats;
        _awaitingUpgradeConfirmation = false;

        UpdateAll();
    }

    public void Hide()
    {
        _currentTile = null;
        _currentTowerStats = null;
        _awaitingUpgradeConfirmation = false;
    }

    public void Update()
    {
        if (_currentTile == null)
            return;

        UpdateExperience();
        UpdateTotalStats();
    }

    private void UpdateAll()
    {
        UpdateBasicInfo();
        RefreshStats();
        UpdateUpgradePrice();
        UpdateSellPrice();
        UpdateExperience();
        UpdateTotalStats();

        if (_currentTile != null)
        {
            CurrentLevelText.Value = $"<size=18>LEVEL:</size> {_currentTile.MyTower.UpgradeLevel}";
        }
    }

    private void UpdateBasicInfo()
    {
        TowerName.Value = $"<color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>{_currentTile.MyTower.ShopItemConfig.Name}</color>";
    }

    private void RefreshStats()
    {
        if (_currentTowerStats == null)
            return;

        List<StatData> stats = _currentTowerStats.GetStats();
        StatsText.Value = FormatStats(stats);
        UpgradeButtonText.Value = "UPGRADE";
        UpgradeButtonColor.Value = Utils.HexToColor("#5F98FF");
    }

    private void ShowUpgradePreview()
    {
        if (_currentTowerStats == null)
            return;

        if (_currentTile.MyTower.UpgradeLevel < _currentTile.MyTower.MaxUpgrades)
        {
            List<StatData> upgradedStats = _currentTowerStats.GetStatsAfterUpgrade();
            StatsText.Value = FormatStats(upgradedStats);
            UpdateUpgradePrice();
        }
        else
        {
            UpgradePrice.Value = "MAX";
        }
    }

    private string FormatStats(List<StatData> stats)
    {
        if (stats == null || stats.Count == 0)
            return string.Empty;

        string formatted = string.Empty;
        string colorHex = $"<color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>";

        foreach (StatData stat in stats)
        {
            formatted += $"{stat.Name}: {colorHex}{stat.Value}</color>\n";
        }

        return formatted;
    }

    private void UpdateUpgradePrice()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        UpgradePrice.Value = _currentTile.MyTower.UpgradeLevel < _currentTile.MyTower.MaxUpgrades
            ? Utils.FormatNumber(_currentTile.MyTower.GetNextUpgradePrice(), '.').ToString()
            : "MAX";
    }

    private void UpdateSellPrice()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        int sellPrice = Mathf.RoundToInt(_currentTile.MyTower.TotalInvested * _currentTile.MyTower.ShopItemConfig.SellMultiplier);
        SellPrice.Value = Utils.FormatNumber(sellPrice, '.').ToString();
    }

    private void UpdateExperience()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        string autoLevel = $"<color={Utils.ColorToHex(_currentTile.MyTower.ShopItemConfig.NameColor)}>L{_currentTile.MyTower.AutomaticLevel}</color>";
        string XPProgress = $"{Utils.FormatCompactNumber(_currentTile.MyTower.CurrentExpirience)} / <color=#FF9700>{Utils.FormatCompactNumber(_currentTile.MyTower.ExpirienceToNextLevel)}</color>";

        AutoLevelText.Value = $"{autoLevel}\n{XPProgress}\nXP";

        UpgradeSliderMax.Value = _currentTile.MyTower.MaxUpgrades;
        UpgradeSliderValue.Value = _currentTile.MyTower.UpgradeLevel;
        LevelSliderMax.Value = _currentTile.MyTower.ExpirienceToNextLevel;
        LevelSliderValue.Value = _currentTile.MyTower.CurrentExpirience;
    }

    private void UpdateTotalStats()
    {
        if (_currentTile == null || _currentTile.MyTower == null)
            return;

        TotalDamage.Value = Utils.FormatCompactNumber(_currentTile.MyTower.TotalDamageDeal);
        TotalEnemiesKilled.Value = _currentTile.MyTower.TotalEnemyKilled.ToString();
        TotalXPEarned.Value = Utils.FormatCompactNumber(_currentTile.MyTower.TotalExpirience);
    }

    public void Initialize()
    {
        SellCommand.Subscribe(_ => SellTower());
        UpgradeCommand.Subscribe(_ => UpgradeTower());
    }

    private void SellTower()
    {
        if (_currentTile == null)
            return;

        Bootstrapper.Instance.EventBus.RaiseSellTower(_currentTile);
    }

    private void UpgradeTower()
    {
        if (_currentTile == null || _currentTile.MyTower == null || _currentTile.MyTower.UpgradeLevel >= _currentTile.MyTower.MaxUpgrades)
            return;

        if (!_awaitingUpgradeConfirmation)
        {
            _awaitingUpgradeConfirmation = true;
            ShowUpgradePreview();
            UpgradeButtonText.Value = "CONFIRM";
            UpgradeButtonColor.Value = Color.green;
        }
        else
        {
            Bootstrapper.Instance.EventBus.RaiseUpgradeTower(_currentTile);
            _awaitingUpgradeConfirmation = false;
            RefreshStats();
            UpdateUpgradePrice();
            UpgradeButtonText.Value = "UPGRADE";
            UpgradeButtonColor.Value = Utils.HexToColor("#5F98FF");
        }
    }
}