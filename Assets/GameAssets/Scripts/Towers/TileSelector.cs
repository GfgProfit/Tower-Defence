using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private VisualCircle _visualCircle;
    [SerializeField] private RectTransform _shopPanelTransform;
    [SerializeField] private TowerStatsPanel _statsPanelTransform;

    private Camera _mainCamera;

    public TowerTile SelectedTile { get; private set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleEscapeInput();
        HandleMouseClick();
    }

    private void HandleEscapeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SelectedTile != null)
        {
            DeselectTile();
        }
    }

    private void HandleMouseClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out TowerTile tile))
            {
                ProcessTileClick(tile);
            }
        }
    }

    private void ProcessTileClick(TowerTile tile)
    {
        if (tile == SelectedTile)
        {
            DeselectTile();
        }
        else
        {
            SelectTile(tile);
        }
    }

    private void SelectTile(TowerTile tile)
    {
        if (SelectedTile != null)
        {
            DeselectTile();
        }

        SelectedTile = tile;
        SelectedTile.Select();

        ShowVisualCircle();
        ShowUI();
    }

    private void DeselectTile()
    {
        SelectedTile.Deselect();
        SelectedTile = null;

        HideVisualCircle();
        _shopPanelTransform.gameObject.SetActive(false);
        CloseStatsPanel();
    }

    public void ShowUI()
    {
        if (SelectedTile == null)
        {
            return;
        }

        bool hasTower = SelectedTile.MyTower != null;

        _shopPanelTransform.gameObject.SetActive(!hasTower);

        if (hasTower)
        {
            ShowStatsPanel(SelectedTile.MyTower.ShopItemConfig);
        }
    }

    public void ShowStatsPanel(ShopItemConfig shopItemConfig)
    {
        if (shopItemConfig == null)
        {
            return;
        }

        List<StatData> statDatas = shopItemConfig.GetTowerStats();
        string stats = string.Empty;
        string towerName = $"<color={Utils.ColorToHex(shopItemConfig.NameColor)}>{shopItemConfig.Name}</color>";

        foreach (StatData statData in statDatas)
        {
            stats += $"{statData.Name}: <color={Utils.ColorToHex(shopItemConfig.NameColor)}>{statData.Value}</color>\n";
        }

        _statsPanelTransform.Show(shopItemConfig.TowerIcon, towerName, stats);
    }

    public void CloseStatsPanel() => _statsPanelTransform.Hide();
    public void ShowVisualCircle() => _visualCircle.Show(SelectedTile);
    public void HideVisualCircle() => _visualCircle.Hide();
}