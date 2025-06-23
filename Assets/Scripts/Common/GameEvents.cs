using System;
using UnityEngine;

using Common.Interfaces;
using Tower;
using Wave;

namespace Common
{
    public static class GameEvents
    {
        /* Events are sorted by the classes that invoke them */

        // Enemy
        public static Action<IDamageable> OnEnemyDeath;
        public static Action OnEnemyBreached;

        // WaveManager
        public static Action OnWaveComplete;

        // GameManager
        public static Action OnInitializeGameData;
        public static Action<float> OnGameStart;
        public static Action<WaveSO> OnWaveStart;
        public static Action<float, bool> OnUpdateTimer;
        public static Action OnLevelComplete;
        public static Action OnGameOver;

        // MoneySystem
        public static Action OnMoneyChanged;

        // UI
        public static Action<TowerSO, Transform> OnTowerBought;
        public static Action<TowerSO> OnTowerUpgrade;
        public static Action<int> OnTowerSold;
        public static Action<float> OnCallWaveEarly;

        // TowerSlot
        public static Action<TowerSlot> OnTowerSlotSelected;
    }
}
