using UnityEngine;
using GameAssets.Global.Core;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopUIManager _shopUIManager;
    [SerializeField] private TileSelector _tileSelector;

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnShopBuy += HandleShopBuy;
        GameController.Instance.EventBus.OnSellTower += HandleSellTower;
        GameController.Instance.EventBus.OnUpgradeTower += HandleUpgradeTower;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnShopBuy -= HandleShopBuy;
        GameController.Instance.EventBus.OnSellTower -= HandleSellTower;
        GameController.Instance.EventBus.OnUpgradeTower -= HandleUpgradeTower;
    }

    private void HandleShopBuy(ShopItemConfig shopItemConfig)
    {
        int currentMoney = GameController.Instance.EventBus.RaiseRequestMoney();

        if (currentMoney < shopItemConfig.Price)
        {
            return;
        }

        GameController.Instance.EventBus.RaiseMoneySpend(shopItemConfig.Price);

        var tile = _tileSelector.SelectedTile;
        if (tile == null)
        {
            return;
        }

        TowerBase tower = Instantiate(
            shopItemConfig.TowerPrefab,
            tile.TowerSpawnPoint.position,
            Quaternion.identity
        );

        tower.transform.localScale = Vector3.zero;
        tower.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        tower.AddInvestment(shopItemConfig.Price);

        tile.SetTower(tower);

        GameController.Instance.EventBus.RaiseTileSelected(tile);
    }

    private void HandleSellTower(TowerTile tile)
    {
        if (tile == null || tile.MyTower == null)
        {
            return;
        }

        int sellPrice = Mathf.RoundToInt(tile.MyTower.TotalInvested * tile.MyTower.ShopItemConfig.SellMultiplier);

        GameController.Instance.EventBus.RaiseMoneyGather(sellPrice);

        Destroy(tile.MyTower.gameObject);
        tile.SetTower(null);
        _tileSelector.DeselectTile();

        GameController.Instance.EventBus.RaiseTileDeselected(tile);
    }

    private void HandleUpgradeTower(TowerTile tile)
    {
        if (tile == null || tile.MyTower == null)
            return;

        if (tile.MyTower.UpgradeLevel < 15)
        {
            int price = tile.MyTower.GetNextUpgradePrice();
            int playerMoney = GameController.Instance.EventBus.RaiseRequestMoney();

            if (playerMoney < price)
                return;

            GameController.Instance.EventBus.RaiseMoneySpend(price);
            tile.MyTower.AddInvestment(price);
            tile.MyTower.Upgrade();

            GameController.Instance.EventBus.RaiseTileSelected(tile);
        }
    }
}