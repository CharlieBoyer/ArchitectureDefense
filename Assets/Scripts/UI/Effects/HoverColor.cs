using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Effects
{
    public class HoverColor: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private List<Graphic> _graphics;

        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _hoverColor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_graphics.Count <= 0) return;

            foreach (Graphic item in _graphics)
                item.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_graphics.Count <= 0) return;

            foreach (Graphic item in _graphics)
                item.color = _baseColor;
        }
    }
}