using DG.Tweening;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    [SerializeField] private Transform _towerSpawnPoint;
    [SerializeField] private Renderer _tileRenderer;
    [SerializeField] private Color _baseColor = Color.white;

    public TowerBase MyTower { get; private set; }
    public Transform TowerSpawnPoint => _towerSpawnPoint;

    public void Select()
    {
        _tileRenderer.material.color = Color.green;
    }

    public void Deselect()
    {
        _tileRenderer.material.color = _baseColor;
    }

    public void SetTower(TowerBase tower) => MyTower = tower;
}