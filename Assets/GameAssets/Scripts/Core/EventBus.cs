using System;

namespace GameAssets.Global.Core
{
    public class EventBus
    {
        public event Action OnGameOver;
        public void RaiseGameOver() => OnGameOver?.Invoke();

        public event Action<int, Action> OnMoneySpend;
        public void RaiseMoneySpend(int amount, Action callback) => OnMoneySpend?.Invoke(amount, callback);
        
        public event Action<int> OnMoneyGather;
        public void RaiseMoneyGather(int amount) => OnMoneyGather?.Invoke(amount);
        
        public event Action<float> OnPortalTakeDamage;
        public void RaisePortalTakeDamage(float amount) => OnPortalTakeDamage?.Invoke(amount);
    }
}