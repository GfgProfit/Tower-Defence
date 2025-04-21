using UnityEngine;
using GameAssets.Global.Core;

public class TileUIManager : MonoBehaviour
{
    [SerializeField] private VisualCircle _visualCircle;
    [SerializeField] private RectTransform _shopPanelTransform;
    [SerializeField] private TowerStatsPanel _statsPanelTransform;

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnTileSelected += HandleTileSelected;
        GameController.Instance.EventBus.OnTileDeselected += HandleTileDeselected;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnTileSelected -= HandleTileSelected;
        GameController.Instance.EventBus.OnTileDeselected -= HandleTileDeselected;
    }

    private void HandleTileSelected(TowerTile tile)
    {
        if (tile.MyTower == null)
        {
            _shopPanelTransform.gameObject.SetActive(true);
            _statsPanelTransform.Hide();
        }
        else
        {
            _shopPanelTransform.gameObject.SetActive(false);
            ShowStatsPanel(tile.MyTower.ShopItemConfig, tile);
            _statsPanelTransform.RefreshStats();
        }

        _visualCircle.Show(tile);
    }
    private void HandleTileDeselected(TowerTile tile)
    {
        _shopPanelTransform.gameObject.SetActive(false);
        _statsPanelTransform.Hide();
        _visualCircle.Hide();
    }

    private void ShowStatsPanel(ShopItemConfig shopItemConfig, TowerTile tile)
    {
        if (shopItemConfig == null)
        {
            return;
        }

        var statDatas = shopItemConfig.GetTowerStats();
        string towerName = $"<color={Utils.ColorToHex(shopItemConfig.NameColor)}>{shopItemConfig.Name}</color>";
        string stats = string.Empty;

        foreach (var statData in statDatas)
        {
            stats += $"{statData.Name}: <color={Utils.ColorToHex(shopItemConfig.NameColor)}>{statData.Value}</color>\n";
        }

        _statsPanelTransform.Show(shopItemConfig.TowerIcon, towerName, stats, tile);
    }
}