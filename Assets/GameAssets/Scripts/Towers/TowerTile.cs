using UnityEngine;

public class TowerTile : MonoBehaviour
{
    [SerializeField] private Transform _towerSpawnPoint;

    private Renderer _renderer;
    private Color _originalColor;
    private TowerBase _myTower;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.sharedMaterial.color;
    }

    public void Select()
    {
        _renderer.sharedMaterial.color = Color.green;
        transform.localScale = new(1.1f, 1.0f, 1.1f);
    }

    public void Deselect()
    {
        _renderer.sharedMaterial.color = _originalColor;
        transform.localScale = Vector3.one;
    }

    public TowerBase GetTowerInTile() => _myTower;
    public Transform GetTowerSpawnPoint() => _towerSpawnPoint;
    public void SetTower(TowerBase tower) => _myTower = tower;
}