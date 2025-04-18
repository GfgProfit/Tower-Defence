using DG.Tweening;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    [SerializeField] private Transform _towerSpawnPoint;
    [SerializeField] private TowerBase _myTower;
    [SerializeField] private Transform _visualCircleTransform;

    private Renderer _renderer;
    private Color _originalColor;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.material.color;
    }

    public void Select()
    {
        _renderer.material.color = Color.green;

        transform.DOScaleX(1.1f, 0.2f).SetEase(Ease.OutBack);
        transform.DOScaleZ(1.1f, 0.2f).SetEase(Ease.OutBack);

        ShowVisualCircle(true);
    }

    public void Deselect()
    {
        ShowVisualCircle(false);

        _renderer.material.color = _originalColor;

        transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutBack);
    }

    public void ShowVisualCircle(bool value)
    {
        if (_myTower == null)
        {
            return;
        }

        _visualCircleTransform.gameObject.SetActive(value);
        _visualCircleTransform.position = _towerSpawnPoint.position;
        _visualCircleTransform.localScale = Vector3.one * _myTower.GetVisionRange();
    }

    public TowerBase GetTowerInTile() => _myTower;
    public Transform GetTowerSpawnPoint() => _towerSpawnPoint;
    public void SetTower(TowerBase tower) => _myTower = tower;
}