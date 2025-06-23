using Common;
using Common.Interfaces;
using UnityEngine;

namespace Economy
{
    public class MoneyManager : MonoBehaviour, ITransaction, IReward
    {
        [SerializeField] private float _callWaveEarlyRewardAmount = 10f;
        [SerializeField, Range(0, 1)] private float _towerSalePercentage = 0.3f;

        private int PlayerMoney => GameData.PlayerMoney; //lecture seul

        private void OnEnable()
        {
            GameEvents.OnCallWaveEarly += CallWaveEarlyReward;
            GameEvents.OnTowerSold += SoldTower;

        }

        private void OnDisable()
        {
            GameEvents.OnCallWaveEarly -= CallWaveEarlyReward;
            GameEvents.OnTowerSold -= SoldTower;
        }

        public void GainMoney(int amount)
        {
            GameData.PlayerMoney += amount;
            GameEvents.OnMoneyChanged?.Invoke();
        }

        public bool CanBuy(int cost)
        {
            return PlayerMoney >= cost;
        }

        public void SpendMoney(int cost)
        {
            if (!CanBuy(cost)) return;

            GameData.PlayerMoney -= cost;
            GameEvents.OnMoneyChanged?.Invoke();
        }

        private void CallWaveEarlyReward(float timeRemaining)
        {
            int reward = Mathf.CeilToInt(timeRemaining * _callWaveEarlyRewardAmount);
            GainMoney(reward);
        }

        private void SoldTower(int cost)
        {
            int reward = Mathf.FloorToInt(cost * _towerSalePercentage);
            GainMoney(reward);
        }
    }
}
