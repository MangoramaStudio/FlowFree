using Managers;
using UnityEngine;

namespace Behaviours
{
    public class CameraFocus : MonoBehaviour
    {
        // Higher is better
        [SerializeField] private int focusPriority;
        [SerializeField] private Transform focalTransform;

        public int FocusPriority
        {
            get => focusPriority;
            set => focusPriority = value;
        }

        public Transform FocalTransform => focalTransform ? focalTransform : transform;

        private void OnEnable()
        {
            CameraFocusManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            CameraFocusManager.Instance.Unregister(this);
        }
    }
}