using DG.Tweening;
using UnityEngine;

public class VisualCircle : MonoBehaviour
{
    [SerializeField] private Transform _visual;

    [Space]
    [SerializeField] private float _yAxisOffset = 0.5125f;

    public void Show(TowerTile selectedTile)
    {
        if (selectedTile == null || selectedTile.MyTower == null)
        {
            return;
        }

        _visual.gameObject.SetActive(true);

        _visual.position = selectedTile.TowerSpawnPoint.position + new Vector3(0, _yAxisOffset, 0);
        Vector3 localScale = Vector3.one * selectedTile.MyTower.GetVisionRange();
        _visual.localScale = Vector3.zero;
        _visual.DOScale(localScale, 0.2f).SetEase(Ease.OutBack).SetLink(_visual.gameObject, LinkBehaviour.RewindOnDisable);
    }

    public void Hide()
    {
        _visual.gameObject.SetActive(false);
    }
}