using UnityEngine;

public class TowerPanel : MonoBehaviour
{
    [SerializeField] private MoneyHandler _moneyHandler;
    [SerializeField] private TileSelector _tileSelector;

    [Space]
    [SerializeField] private TowerCard[] _towerCards;

    private void Awake()
    {
        foreach (TowerCard card in _towerCards)
        {
            card.Init(_moneyHandler, _tileSelector);
        }
    }
}