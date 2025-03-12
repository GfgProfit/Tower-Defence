using UnityEngine;

public class TileSelector : MonoBehaviour
{
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
                    SelectTile(tile);
                }
            }
            else
            {
                if (_selectedTile != null)
                {
                    _selectedTile.Deselect();
                    _selectedTile = null;
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
    }
}
