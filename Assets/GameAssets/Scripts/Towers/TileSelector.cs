using UnityEngine;
using GameAssets.Global.Core;

public class TileSelector : MonoBehaviour
{
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

    public void SelectTile(TowerTile tile)
    {
        if (SelectedTile != null)
        {
            DeselectTile();
        }

        SelectedTile = tile;
        SelectedTile.Select();

        GameController.Instance.EventBus.RaiseTileSelected(SelectedTile);
    }

    public void DeselectTile()
    {
        if (SelectedTile == null)
        {
            return;
        }

        SelectedTile.Deselect();

        GameController.Instance.EventBus.RaiseTileDeselected(SelectedTile);
        SelectedTile = null;
    }
}
