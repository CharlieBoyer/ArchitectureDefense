using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Wave
{
    public class ToDeleteStartWave : MonoBehaviour
    {
        public WaveSO Enemies;
        void Start()
        {
            GameEvents.OnWaveStart.Invoke(Enemies);
        }
    }
}
