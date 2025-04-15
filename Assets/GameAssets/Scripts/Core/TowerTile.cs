using UnityEngine;

public class TowerTile : MonoBehaviour
{
    [SerializeField] private Transform _towerSpawnPoint;

    private Renderer _renderer;
    private Color _originalColor;
    private Tower _myTower;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.material.color;
    }

    public void Select()
    {
        _renderer.material.color = Color.green;
        transform.localScale = new(1.1f, 1.0f, 1.1f);
    }

    public void Deselect()
    {
        _renderer.material.color = _originalColor;
        transform.localScale = Vector3.one;
    }

    public Tower GetTowerInTile() => _myTower;
    public Transform GetTowerSpawnPoint() => _towerSpawnPoint;
    public void SetTower(Tower tower) => _myTower = tower;
}