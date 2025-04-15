using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private RectTransform _towersPanel;

    private TowerTile _selectedTile;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out TowerTile tile))
                {
                    if (tile == _selectedTile)
                    {
                        _selectedTile.Deselect();
                        _selectedTile = null;
                        _towersPanel.gameObject.SetActive(false);

                        return;
                    }

                    SelectTile(tile);
                }
            }
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

        _towersPanel.gameObject.SetActive(true);
    }

    public TowerTile GetSelectedTile() => _selectedTile;
}