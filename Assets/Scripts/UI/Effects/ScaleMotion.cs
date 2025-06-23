using UnityEngine;
using DG.Tweening;

namespace UI.Effects
{
    [ExecuteAlways] [RequireComponent(typeof(RectTransform))]
    public class ScaleMotion : MonoBehaviour
    {
        [SerializeField] private Vector3 _scale = new(0, 0, 5);
        [SerializeField] private float _duration = 2f;

        private RectTransform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            StartAnimation();
        }

        private void StartAnimation()
        {
            _transform.DOScale(_scale, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}