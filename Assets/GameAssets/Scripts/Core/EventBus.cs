using System;

namespace GameAssets.Global.Core
{
    public class EventBus
    {
        public event Action OnGameOver;
        public void RaiseGameOver() => OnGameOver?.Invoke();

        public event Action<int> OnMoneySpend;
        public void RaiseMoneySpend(int amount) => OnMoneySpend?.Invoke(amount);
        
        public event Action<int> OnMoneyGather;
        public void RaiseMoneyGather(int amount) => OnMoneyGather?.Invoke(amount);
        
        public event Action<float> OnPortalTakeDamage;
        public void RaisePortalTakeDamage(float amount) => OnPortalTakeDamage?.Invoke(amount);

        public event Action<ShopItemConfig> OnShopBuy;
        public void RaiseShopBuy(ShopItemConfig shopItemConfig) => OnShopBuy?.Invoke(shopItemConfig);

        public event Func<int> OnRequestMoney;
        public int RaiseRequestMoney() => OnRequestMoney?.Invoke() ?? 0;

        public event Action<TowerTile> OnTileSelected;
        public void RaiseTileSelected(TowerTile tile) => OnTileSelected?.Invoke(tile);

        public event Action<TowerTile> OnTileDeselected;
        public void RaiseTileDeselected(TowerTile tile) => OnTileDeselected?.Invoke(tile);

        public event Action<TowerTile> OnSellTower;
        public void RaiseSellTower(TowerTile tile) => OnSellTower?.Invoke(tile);

        public event Action<TowerTile> OnUpgradeTower;
        public void RaiseUpgradeTower(TowerTile tile) => OnUpgradeTower?.Invoke(tile);
    }
}