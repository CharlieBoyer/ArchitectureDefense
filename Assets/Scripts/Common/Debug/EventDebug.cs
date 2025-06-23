using UnityEngine;

namespace Common.Debug
{
    public class EventDebug: MonoBehaviour
    {
        [ContextMenu("Debug [OnMoneyChanged]")]
        public void GiveMoney()
        {
            GameData.PlayerMoney += 100;
            GameEvents.OnMoneyChanged?.Invoke();
        }

        [ContextMenu("Debug [OnEnemyBreached]")]
        public void EnemyBreached()
        {
            GameData.PlayerLives--;
            GameEvents.OnEnemyBreached?.Invoke();
        }

        [ContextMenu("Debug [OnWaveComplete]")]
        public void WaveComplete()
        {
            GameEvents.OnWaveComplete?.Invoke();
        }


    }
}