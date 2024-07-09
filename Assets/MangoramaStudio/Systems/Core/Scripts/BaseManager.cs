using System;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers
{

    public class BaseManager : MonoBehaviour
    {
        public GameManager GameManager => GameManager.Instance;

        public virtual void Initialize()
        {
            ToggleEvents(true);
        }

        public void OnDestroy()
        {
            ToggleEvents(false);
        }

        protected virtual void ToggleEvents(bool isToggled) { }
    }
}