using DG.Tweening;
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

        _originalColor = _renderer.material.color;
    }

    public void Select()
    {
        _renderer.material.color = Color.green;

        transform.DOScaleX(1.1f, 0.1f).SetEase(Ease.OutBack);
        transform.DOScaleZ(1.1f, 0.1f).SetEase(Ease.OutBack);
    }

    public void Deselect()
    {
        _renderer.material.color = _originalColor;

        transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutBack);
    }

    public TowerBase GetTowerInTile() => _myTower;
    public Transform GetTowerSpawnPoint() => _towerSpawnPoint;
    public void SetTower(TowerBase tower) => _myTower = tower;
}