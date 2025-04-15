using UnityEngine.EventSystems;
using GameAssets.Global.Core;
using UnityEngine;
using TMPro;

public class TowerCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;

    [Space]
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private string _name;
    [SerializeField] private int _price;

    private MoneyHandler _moneyHandler;
    private TileSelector _tileSelector;

    public void Init(MoneyHandler moneyHandler, TileSelector tileSelector)
    {
        _nameText.text = _name;
        _priceText.text = $"<color=#FFC87F>$:</color> {_price}";

        _moneyHandler = moneyHandler;
        _tileSelector = tileSelector;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_moneyHandler.Money >= _price)
        {
            if (_tileSelector.GetSelectedTile().GetTowerInTile() == null)
            {
                GameManager.Instance.EventBus.OnMoneySpend?.Invoke(_price);

                TowerTile selectedTile = _tileSelector.GetSelectedTile();
                Transform selectedTowerSpawnPoint = selectedTile.GetTowerSpawnPoint();

                Tower tower = Instantiate(_towerPrefab, selectedTowerSpawnPoint.position, selectedTowerSpawnPoint.rotation);

                selectedTile.SetTower(tower);
            }
        }
    }
}