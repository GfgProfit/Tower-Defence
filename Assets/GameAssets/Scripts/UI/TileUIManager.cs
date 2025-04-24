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
            ShowStatsPanel(tile);
        }

        _visualCircle.Show(tile);
    }
    private void HandleTileDeselected(TowerTile tile)
    {
        _shopPanelTransform.gameObject.SetActive(false);
        _statsPanelTransform.Hide();
        _visualCircle.Hide();
    }

    private void ShowStatsPanel(TowerTile tile)
    {
        if (tile == null)
        {
            return;
        }

        _statsPanelTransform.Show(tile);
    }
}