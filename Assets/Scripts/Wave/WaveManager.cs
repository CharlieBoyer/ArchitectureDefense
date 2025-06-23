using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Wave
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject _enemySpawn;
        [SerializeField] private GameObject _enemyDestination;

        private List<GameObject> _enemyList;
        private float _spawnDelay;

        private int _enemyDeathCount;

        private void OnEnable()
        {
            GameEvents.OnWaveStart += HandleWaveStart;
            GameEvents.OnEnemyDeath += HandleEnemyDeath;
            GameEvents.OnEnemyBreached += HandleEnemyBreach;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStart -= HandleWaveStart;
            GameEvents.OnEnemyDeath -= HandleEnemyDeath;
            GameEvents.OnEnemyBreached -= HandleEnemyBreach;
        }

        private void HandleWaveStart(WaveSO wave)
        {
            _enemyList = wave.EnemyUnits.ToList();
            _spawnDelay = GameData.WaveInternalDelay * wave.SpawnDelayModifier;

            StartCoroutine(SpawnWaveCoroutine(_enemyDestination));
        }

        private void HandleEnemyDeath(IDamageable obj)
        {
            TrackList();
        }

        private void HandleEnemyBreach()
        {
            TrackList();
        }

        private IEnumerator SpawnWaveCoroutine(GameObject destination)
        {
            foreach (GameObject enemyPrefab in _enemyList)
            {
                GameObject enemy = Instantiate(enemyPrefab, _enemySpawn.transform.position, Quaternion.identity);

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

                if (agent)
                    agent.destination = destination.transform.position;

                yield return new WaitForSeconds(_spawnDelay);
            }
        }

        private void TrackList()
        {
            _enemyDeathCount++;

            if (_enemyDeathCount == _enemyList.Count)
            {
                _enemyDeathCount = 0;
                GameEvents.OnWaveComplete.Invoke();
            }
        }

    }
}
