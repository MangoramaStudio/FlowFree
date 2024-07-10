using Behaviours;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MangoramaStudio.Game.Common.Scripts.CanvasHandlers.Scripts
{
    public class CanvasRenderCameraHandler : UIBehaviour
    {
        private Canvas Canvas => _canvas ? _canvas : (_canvas = GetComponent<Canvas>());
        private Canvas _canvas;

        protected override void OnEnable()
        {
            base.OnEnable();
            Canvas.worldCamera = UiCamera.Instance.Camera;
        }
        
    }
}