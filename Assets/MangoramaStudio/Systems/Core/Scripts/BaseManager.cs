using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{

    public class BaseManager : MonoBehaviour
    {
    
        public GameManager GameManager => GameManager.Instance;

        public virtual void Initialize()
        {
            ToggleEvents(true);
        }

        public virtual void OnDestroy()
        {
            ToggleEvents(false);
        }

        protected virtual void ToggleEvents(bool isToggled) { }
    }
}