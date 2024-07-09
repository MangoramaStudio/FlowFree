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
    
    public class BaseMenu : UIBehaviour
    {
        [SerializeField] private MenuType menuType;

        public MenuType MenuType => menuType;
    }
}