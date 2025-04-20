using System.Collections.Generic;
using GameAssets.Global.Core;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItemConfig[] _shopItemConfigs;

    [Space]
    [SerializeField] private RectTransform _shopPanelTransform;
    [SerializeField] private ShopItemUI _shopItemUIPrefab;
    [SerializeField] private TileSelector _tileSelector;

    public TileSelector TileSelector => _tileSelector;

    private void Awake()
    {
        InitShop();
    }

    private void OnEnable() => GameController.Instance.EventBus.OnShopBuy += Buy;
    private void OnDisable() => GameController.Instance.EventBus.OnShopBuy -= Buy;

    private void InitShop()
    {
        _shopPanelTransform.gameObject.SetActive(true);

        for (int i = 0; i < _shopItemConfigs.Length; i++)
        {
            ShopItemUI shopItem = Instantiate(_shopItemUIPrefab, _shopPanelTransform);

            shopItem.Setup(_shopItemConfigs[i]);
        }

        _shopPanelTransform.gameObject.SetActive(false);
    }

    private void Buy(ShopItemConfig shopItemConfig)
    {
        int money = GameController.Instance.EventBus.RaiseRequestMoney();

        if (money >= shopItemConfig.Price)
        {
            GameController.Instance.EventBus.RaiseMoneySpend(shopItemConfig.Price);

            TowerBase towerBase = Instantiate(shopItemConfig.TowerPrefab, _tileSelector.SelectedTile.TowerSpawnPoint.position, Quaternion.identity);
            _tileSelector.SelectedTile.SetTower(towerBase);
            _tileSelector.ShowVisualCircle();
            _tileSelector.ShowUI();
        }
    }
}