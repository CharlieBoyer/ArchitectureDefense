using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraBinder: MonoBehaviour
    {
        private void Awake()
        {
            Camera mainCamera = Camera.main;
            Canvas canvas = GetComponent<Canvas>();

            if (mainCamera && canvas)
                canvas.worldCamera = mainCamera;
        }
    }
}