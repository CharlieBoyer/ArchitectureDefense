using UnityEngine;
using DG.Tweening;

namespace UI.Effects
{
    [ExecuteAlways] [RequireComponent(typeof(RectTransform))]
    public class MoveMotion : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new(1.5f, 1.5f, 1.5f);
        [SerializeField] private float _duration = 2f;

        private RectTransform _transform;
        private Vector3 _startPosition;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _startPosition = _transform.position;

            StartAnimation();
        }

        private void StartAnimation()
        {
            _transform.DOMove(_startPosition + _offset, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
