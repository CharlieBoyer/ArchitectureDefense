using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Tower
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private List<TowerSO> _availableTowers;

        private void Awake()
        {
            GameData.Towers = _availableTowers;
        }

        private void OnEnable()
        {
            GameEvents.OnTowerBought += BuildTower;
            //GameEvents.OnSellTower += SellTower;
        }
        private void OnDisable()
        {
            GameEvents.OnTowerBought -= BuildTower;
            //GameEvents.OnSellTower -= SellTower;
        }

        [ContextMenu("Build")]
        private void BuildTower(TowerSO towerSoToBuild, Transform towerLocation)
        {
            GameObject towerToBuildPrefab = towerSoToBuild.TowerPrefab;
            Instantiate(towerToBuildPrefab, towerLocation.position, towerLocation.rotation);
        }
        private int SellTower(GameObject tower)
        {
            int value = tower.GetComponent<Tower>().SellTower();
            Destroy(gameObject);
            return value;
        }
    }
}
