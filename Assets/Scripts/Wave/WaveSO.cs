using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "Wave_", menuName = "SO/Wave")]
    public class WaveSO : ScriptableObject
    {
        [SerializeField] private int _waveID;
        [SerializeField] private float _spawnDelayModifier = 1f;
        [SerializeField] private List<GameObject> _enemyUnits = new List<GameObject>();

        public int WaveID => _waveID;
        public float SpawnDelayModifier => _spawnDelayModifier;
        public List<GameObject> EnemyUnits => _enemyUnits;
    }
}
