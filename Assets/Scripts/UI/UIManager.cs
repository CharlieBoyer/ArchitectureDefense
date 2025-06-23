using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

using Common;
using Tower;
using UI.Modules;
using UnityEngine.UI;
using Wave;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private static readonly int CurrentValue = Shader.PropertyToID("_CurrentValue");
        private static readonly int EnableMarquee = Shader.PropertyToID("_EnableMarquee");

        [Header("References")]
        [SerializeField] private TMP_Text _waveCounter;
        [SerializeField] private TMP_Text _moneyCounter;
        [SerializeField] private TMP_Text _livesCounter;
        [SerializeField] private Slider _waveTimer;
        [SerializeField] private Graphic _waveTimerFill;
        [SerializeField] private TMP_Text _timerCounter;
        [SerializeField] private Button _callWaveButton;
        [Space]
        [SerializeField] private GameObject _towerConstructionTooltipPrefab;

        [Header("Animation Settings")]
        [SerializeField] private float _moneyUpdateDuration = 1f;
        [SerializeField] private float _livesUpdateDuration = 1f;
        [SerializeField] private int _livesUpdateRepetitions = 5;
        [SerializeField] private float _waveUpdateSpeed = 10f;
        [SerializeField] private int _waveUpdateRepetitions = 20;

        private ConstructionTooltip _constructionTooltip;
        private TowerSlot _selectedTowerSlot;

        private Tween _livesUpdater;
        private Tween _moneyUpdater;

        private void OnEnable()
        {
            GameEvents.OnInitializeGameData += OnInitializeGameData;
            GameEvents.OnGameStart += OnGameStart;
            GameEvents.OnMoneyChanged += OnMoneyChanged;
            GameEvents.OnEnemyBreached += OnEnemyBreached;
            GameEvents.OnWaveStart += OnWaveStart;
            GameEvents.OnWaveComplete += OnWaveComplete;
            GameEvents.OnLevelComplete += OnLevelComplete;
            GameEvents.OnTowerSlotSelected += OnTowerSlotSelected;
            GameEvents.OnUpdateTimer += OnUpdateTimer;

            _callWaveButton.onClick.AddListener(OnCallWaveEarly);
        }

        void OnDisable()
        {
            GameEvents.OnInitializeGameData -= OnInitializeGameData;
            GameEvents.OnGameStart -= OnGameStart;
            GameEvents.OnMoneyChanged -= OnMoneyChanged;
            GameEvents.OnEnemyBreached -= OnEnemyBreached;
            GameEvents.OnWaveStart -= OnWaveStart;
            GameEvents.OnWaveComplete -= OnWaveComplete;
            GameEvents.OnLevelComplete -= OnLevelComplete;
            GameEvents.OnTowerSlotSelected -= OnTowerSlotSelected;
            GameEvents.OnUpdateTimer -= OnUpdateTimer;

            _callWaveButton.onClick.RemoveListener(OnCallWaveEarly);
        }

        private void OnInitializeGameData()
        {
            _waveTimer.maxValue = GameData.WaveTimer;
            _livesCounter.text = GameData.PlayerLives.ToString("D2");
            OnMoneyChanged();
        }

        private void OnGameStart(float initialTimer)
        {
            _waveTimer.maxValue = initialTimer;
            _waveTimer.value = initialTimer;
        }

        private void OnUpdateTimer(float timer, bool enableMarquee)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(timer);

            if (enableMarquee)
            {
                _timerCounter.text = "wave ongoing...";
                _waveTimer.value = _waveTimer.maxValue;
                _waveTimerFill.material.SetFloat(CurrentValue, _waveTimer.maxValue);
                _waveTimerFill.material.SetInt(EnableMarquee, 1);
                return;
            }

            _waveTimerFill.material.SetInt(EnableMarquee, 0);
            _waveTimer.value = timer;
            _waveTimerFill.material.SetFloat(CurrentValue, timer);
            _timerCounter.text = timespan.ToString("mm':'ss");
        }

        private void OnMoneyChanged()
        {
            int updatedMoney = GameData.PlayerMoney;
            int currentMoney = int.Parse(_moneyCounter.text.Replace(".", ""));

            if (_moneyUpdater != null && _moneyCounter.IsActive())
                _moneyUpdater.Kill();

            _moneyCounter.color = Color.yellow;

            _moneyUpdater = DOTween.To(() => currentMoney, x => currentMoney = Mathf.RoundToInt(x), updatedMoney, _moneyUpdateDuration)
                .SetEase(Ease.OutQuad)
                .OnUpdate(() => {
                    _moneyCounter.text = FormatMoney(currentMoney);
                })
                .OnComplete(() => _moneyCounter.color = Color.white);
        }

        private void OnEnemyBreached()
        {
            _livesCounter.text = GameData.PlayerLives.ToString("D2");

            if (_livesUpdater != null && _livesCounter.IsActive())
                _livesUpdater.Kill();

            _livesCounter.transform.DOScale(Vector3.one * 1.2f, _livesUpdateDuration / 2)
                .SetLoops(_livesUpdateRepetitions * 2, LoopType.Yoyo);

            _livesUpdater = _livesCounter.DOColor(Color.red, _livesUpdateDuration)
                .SetLoops(_livesUpdateRepetitions, LoopType.Yoyo).OnComplete(() => {
                    _livesCounter.DOColor(Color.white, _livesUpdateDuration);
                    _livesCounter.DOKill(true);
                });
        }

        private void OnWaveStart(WaveSO wave)
        {
            _waveCounter.text = wave.WaveID.ToString();

            _waveCounter.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
            _waveCounter.DOColor(Color.cyan, 1 / _waveUpdateSpeed).SetLoops(_waveUpdateRepetitions, LoopType.Yoyo);
        }

        private void OnWaveComplete()
        {
            // Show "Wave Complete" message
        }

        private void OnLevelComplete()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }

        private void OnCallWaveEarly()
        {
            float remainingTimePercentage = _waveTimer.value / _waveTimer.maxValue;

            GameEvents.OnCallWaveEarly?.Invoke(remainingTimePercentage);
        }

        private void OnTowerSlotSelected(TowerSlot towerSlot)
        {
            if (_constructionTooltip && towerSlot == _selectedTowerSlot)
            {
                _selectedTowerSlot = null;
                _constructionTooltip.Destroy();
                return;
            }

            if (_constructionTooltip)
                _constructionTooltip.Destroy();

            _selectedTowerSlot = towerSlot;

            if (!_selectedTowerSlot.IsEmpty)
                return;

            _constructionTooltip = Instantiate(_towerConstructionTooltipPrefab, towerSlot.transform).GetComponent<ConstructionTooltip>();
            _constructionTooltip.Set(towerSlot);
        }

        private string FormatMoney(int value)
        {
            string valueString = value.ToString();

            if (valueString.Length <= 3)
                return "0." + valueString.PadLeft(3, '0');

            return valueString.Substring(0, valueString.Length - 3) + "." + valueString.Substring(valueString.Length - 3);
        }
    }
}
