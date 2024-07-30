using UnityEngine;
using UnityEngine.EventSystems;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public enum MenuType
    {
        Main,
        Gameplay,
        Win
    }
    
    public  class BaseMenu : UIBehaviour
    {
        [SerializeField] private MenuType menuType;

        public MenuType MenuType => menuType;
        
        public bool IsInitialized { get; set; }

        public virtual void Initialize()
        {
            ToggleEvents(true);
            IsInitialized = true;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            ToggleEvents(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ToggleEvents(false);
        }

        protected virtual void ToggleEvents(bool isToggled)
        {
           
        }
    }
}