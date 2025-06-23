using System.Collections.Generic;
using Tower;

namespace Common
{
    public static class GameData
    {
        public static int PlayerLives = 10;
        public static int PlayerMoney = 0;

        public static float WaveTimer;
        public static float WaveInternalDelay = 1f;

        public static List<TowerSO> Towers = new();
    }
}
