using System;

namespace GameAssets.Global.Core
{
    public class EventBus
    {
        private GameManager _gameManager;

        public EventBus(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public Action OnGameOver;
        public Action<int> OnMoneySpend;
        public Action<int> OnMoneyGather;
    }
}