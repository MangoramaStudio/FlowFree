using System;
using UnityEngine;

namespace Behaviours
{
    public class ForceViewportResolution : MonoBehaviour
    {
        [SerializeField] private Vector2 referenceResolution;
        [SerializeField] private Camera targetCamera;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            var ratio = referenceResolution.x / referenceResolution.y;
            var currentRatio = (float) targetCamera.pixelWidth / targetCamera.pixelHeight;

            if (Mathf.Abs(ratio - currentRatio) > 0.1f)
            {
                var targetWidth = targetCamera.pixelHeight * ratio;
                var viewportWidthRatio = targetWidth / targetCamera.pixelWidth;
                targetCamera.rect = new Rect((1 - viewportWidthRatio) * 0.5f, 0, viewportWidthRatio, 1);
            }
        }
    }
}