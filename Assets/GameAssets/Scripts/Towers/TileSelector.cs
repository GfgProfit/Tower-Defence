using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private RectTransform _towerShopPanel;
    [SerializeField] private RectTransform _towerStatsPanel;

    private TowerTile _selectedTile;
    private Camera _mainCamera;

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
        if (Input.GetKeyDown(KeyCode.Escape) && _selectedTile != null)
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
        if (tile == _selectedTile)
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
        if (_selectedTile != null)
        {
            _selectedTile.Deselect();
        }

        _selectedTile = tile;
        _selectedTile.Select();

        _towerShopPanel.gameObject.SetActive(true);
    }

    private void DeselectTile()
    {
        _selectedTile.Deselect();
        _selectedTile = null;
        _towerShopPanel.gameObject.SetActive(false);
        _towerStatsPanel.gameObject.SetActive(false);
    }

    public TowerTile GetSelectedTile() => _selectedTile;
}