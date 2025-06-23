using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using Tower;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Modules
{
    public class ConstructionTooltip: MonoBehaviour, IPointerExitHandler
    {
        [SerializeField] private Transform _layoutRoot;
        [SerializeField] private GameObject _towerPinPrefab;

        private List<TowerSO> _towers = GameData.Towers;

        private RectTransform _rect;
        private bool _isSpawning = false;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !RectTransformUtility.RectangleContainsScreenPoint(_rect, Input.mousePosition))
            {
                this.Destroy();
            }
        }

        public void Set(TowerSlot towerSlot)
        {
            _isSpawning = true;

            foreach (TowerSO towerType in _towers)
            {
                BlueprintButton blueprintButton = Instantiate(_towerPinPrefab, _layoutRoot).GetComponent<BlueprintButton>();
                blueprintButton.SetupButton(towerType, towerSlot);
            }

            transform.DOScale(Vector3.one, 0.5f).From(0).SetEase(Ease.OutBounce).OnComplete(() => {
                _isSpawning = false;
            });
        }

        private void OnEnable()
        {
            GameEvents.OnTowerBought += Destroy;
        }

        private void OnDisable()
        {
            GameEvents.OnTowerBought -= Destroy;
        }

        public void Destroy(TowerSO tower = null, Transform slot = null)
        {
            if (_isSpawning)
                return;

            StartCoroutine(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            yield return transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).WaitForCompletion();

            transform.DOKill(true);

            Destroy(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.reentered)
                return;

            this.Destroy();
        }
    }
}
