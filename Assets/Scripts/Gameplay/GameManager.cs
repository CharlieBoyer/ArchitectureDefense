using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Wave;

namespace Gameplay
{
    public class GameManager: MonoBehaviour
    {
        [Header("Game Data")]
        [SerializeField] private int _playerLives;
        [SerializeField] private int _playerMoney;
        [SerializeField] private float _waveTimer;
        [SerializeField] private float _waveInternalSpawnDelay;

        [Header("Wave Data")]
        [SerializeField] private List<WaveSO> _waves;

        private Queue<WaveSO> _waveList = new();

        private Coroutine _transitionToNextWave;

        private bool _waveOngoing = false;

        private void Awake()
        {
            _waveList = new Queue<WaveSO>(_waves);
            SetupGameData();
        }

        private void Start()
        {
            GameEvents.OnInitializeGameData?.Invoke();
            GameEvents.OnGameStart?.Invoke(_waveTimer);

            _transitionToNextWave = StartCoroutine(StartNextWaveRoutine(_waveTimer));
        }

        private void OnEnable()
        {
            GameEvents.OnCallWaveEarly += OnCallWaveEarly;
            GameEvents.OnWaveComplete += OnWaveComplete;
            GameEvents.OnWaveStart += OnWaveStart;
        }

        private void OnDisable()
        {
            GameEvents.OnCallWaveEarly -= OnCallWaveEarly;
            GameEvents.OnWaveComplete -= OnWaveComplete;
            GameEvents.OnWaveStart -= OnWaveStart;
        }

        private void SetupGameData()
        {
            GameData.PlayerLives = _playerLives;
            GameData.PlayerMoney = _playerMoney;
            GameData.WaveTimer = _waveTimer;
            GameData.WaveInternalDelay = _waveInternalSpawnDelay;
        }

        private void OnCallWaveEarly(float remainingTimePercentage)
        {
            if (_waveOngoing) return;

            if (_waveList.Count == 0) return;

            if (_transitionToNextWave != null)
                StopCoroutine(_transitionToNextWave);

            GameEvents.OnUpdateTimer?.Invoke(0, true);
            GameEvents.OnWaveStart?.Invoke(_waveList.Dequeue());
        }

        private void OnWaveStart(WaveSO obj)
        {
            _waveOngoing = true;
        }

        private void OnWaveComplete()
        {
            _waveOngoing = false;

            if (_waveList.Count == 0)
            {
                GameEvents.OnLevelComplete?.Invoke();
                return;
            }

            _transitionToNextWave = StartCoroutine(StartNextWaveRoutine(_waveTimer));
        }

        private IEnumerator StartNextWaveRoutine(float delay)
        {
            float timer = delay;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                GameEvents.OnUpdateTimer?.Invoke(timer, false);
                yield return null;
            }

            GameEvents.OnWaveStart?.Invoke(_waveList.Dequeue());
            GameEvents.OnUpdateTimer?.Invoke(0, true);
        }
    }
}
