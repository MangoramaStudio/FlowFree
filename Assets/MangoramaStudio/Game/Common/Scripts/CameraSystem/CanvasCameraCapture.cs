using UnityEngine;

namespace Behaviours
{
    public class CanvasCameraCapture : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CameraEnum camEnum;
        
        private void Awake()
        {
            switch (camEnum)
            {
                case CameraEnum.UiCamera:
                    canvas.worldCamera = UiCamera.Instance.Camera;
                    break;
                case CameraEnum.MainCamera:
                    canvas.worldCamera = Camera.main;
                    break;
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    public enum CameraEnum
    {
        UiCamera,
        MainCamera
    }
}