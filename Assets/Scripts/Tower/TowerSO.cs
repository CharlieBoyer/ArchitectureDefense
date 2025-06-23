using UnityEngine;

namespace Tower
{
    [CreateAssetMenu(fileName = "TowerSO", menuName = "SO/Tower")]
    public class TowerSO : ScriptableObject
    {
        [Header("References")]
        public GameObject TowerPrefab;
        public Sprite TowerIcon;
        public TowerSO UpgradedTower;
        [Header("Price")]
        public int Cost;
        public int SellValue;
        [Header("Statistics")]
        public int Damage;
        public float FireRate;
        public float Range;
    }
}
