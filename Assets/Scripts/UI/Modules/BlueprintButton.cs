using System.Linq;
using Common;
using Common.Interfaces;
using TMPro;
using Tower;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Modules
{
    public class BlueprintButton: MonoBehaviour
    {
        [SerializeField] private Image _towerIcon;
        [SerializeField] private TMP_Text _priceValue;
        [SerializeField] private Button _button;

        private TowerSO _towerBlueprint;
        private TowerSlot _towerSlot;
        private ITransaction _transactionHandler;

        public void SetupButton(TowerSO blueprint, TowerSlot slot)
        {
            _towerBlueprint = blueprint;
            _towerSlot = slot;

            _towerIcon.sprite = _towerBlueprint.TowerIcon;
            _priceValue.text = _towerBlueprint.Cost.ToString();

            _transactionHandler = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ITransaction>().ToArray()[0];

            _button.onClick.AddListener(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            if (_transactionHandler.CanBuy(_towerBlueprint.Cost))
            {
                _transactionHandler.SpendMoney(_towerBlueprint.Cost);
                _towerSlot.CurrentTowerType = _towerBlueprint;
                GameEvents.OnTowerBought?.Invoke(_towerBlueprint, _towerSlot.transform.GetChild(0));
            }
            else
            {
                // Not Enough Gold animation
            }
        }
    }
}
