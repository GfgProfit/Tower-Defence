using UnityEngine;
using GameAssets.Global.Core;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopUIManager _shopUIManager;
    [SerializeField] private TileSelector _tileSelector;

    private void OnEnable()
    {
        Bootstrapper.Instance.EventBus.OnShopBuy += HandleShopBuy;
        Bootstrapper.Instance.EventBus.OnSellTower += HandleSellTower;
        Bootstrapper.Instance.EventBus.OnUpgradeTower += HandleUpgradeTower;
    }

    private void OnDisable()
    {
        Bootstrapper.Instance.EventBus.OnShopBuy -= HandleShopBuy;
        Bootstrapper.Instance.EventBus.OnSellTower -= HandleSellTower;
        Bootstrapper.Instance.EventBus.OnUpgradeTower -= HandleUpgradeTower;
    }

    private void HandleShopBuy(ShopItemConfig shopItemConfig)
    {
        int currentMoney = Bootstrapper.Instance.EventBus.RaiseRequestMoney();

        if (currentMoney < shopItemConfig.Price)
        {
            return;
        }

        Bootstrapper.Instance.EventBus.RaiseMoneySpend(shopItemConfig.Price);

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

        Bootstrapper.Instance.EventBus.RaiseTileSelected(tile);
    }

    private void HandleSellTower(TowerTile tile)
    {
        if (tile == null || tile.MyTower == null)
        {
            return;
        }

        int sellPrice = Mathf.RoundToInt(tile.MyTower.TotalInvested * tile.MyTower.ShopItemConfig.SellMultiplier);

        Bootstrapper.Instance.EventBus.RaiseMoneyGather(sellPrice);

        Destroy(tile.MyTower.gameObject);
        tile.SetTower(null);
        _tileSelector.DeselectTile();

        Bootstrapper.Instance.EventBus.RaiseTileDeselected(tile);
    }

    private void HandleUpgradeTower(TowerTile tile)
    {
        if (tile == null || tile.MyTower == null)
            return;

        if (tile.MyTower.UpgradeLevel < tile.MyTower.MaxUpgrades)
        {
            int price = tile.MyTower.GetNextUpgradePrice();
            int playerMoney = Bootstrapper.Instance.EventBus.RaiseRequestMoney();

            if (playerMoney < price)
                return;

            Bootstrapper.Instance.EventBus.RaiseMoneySpend(price);
            tile.MyTower.AddInvestment(price);
            tile.MyTower.Upgrade();

            Bootstrapper.Instance.EventBus.RaiseTileSelected(tile);
        }
    }
}