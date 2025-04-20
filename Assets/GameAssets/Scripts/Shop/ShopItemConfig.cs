using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item Config", menuName = "Shop/Item Config", order = 0)]
public class ShopItemConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _towerIcon;
    [SerializeField] private Color _nameColor = Color.white;
    [SerializeField] private TowerBase _towerPrefab;
    [SerializeField] private int _price;

    public string Name => _name;
    public Sprite TowerIcon => _towerIcon;
    public Color NameColor => _nameColor;
    public TowerBase TowerPrefab => _towerPrefab;
    public int Price => _price;
    public int SellMoneyValue { get; private set; }

    public List<StatData> GetTowerStats()
    {
        if (TowerPrefab.TryGetComponent(out ITowerStats towerStats))
        {
            return towerStats.GetStats();
        }
        else
        {
            throw new System.Exception($"The <color={Utils.ColorToHex(NameColor)}>{_name}</color> does not implement the ITowerStats interface!");
        }
    }
}