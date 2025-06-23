using UnityEngine;

using Common;

namespace Tower
{
    public class TowerSlot: MonoBehaviour
    {
        private static readonly int Enabled = Shader.PropertyToID("_Enabled");

        public Transform Anchor => transform.Find("Anchor");

        public TowerSO CurrentTowerType { get; set; }
        public bool IsEmpty => CurrentTowerType == null;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void OnMouseDown()
        {
            _renderer.material.SetInt(Enabled, 1);
            GameEvents.OnTowerSlotSelected?.Invoke(this);
        }

        public void OnMouseEnter()
        {
            _renderer.material.SetInt(Enabled, 1);
        }

        public void OnMouseExit()
        {
            _renderer.material.SetInt(Enabled, 0);
        }
    }
}
