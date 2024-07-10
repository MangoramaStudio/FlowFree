using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class GameplayMenu : BaseMenu
    {
        [SerializeField] private GameObject warningObject;
        
        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                GameManager.Instance.EventManager.OnRaiseWarning+=RaiseWarning;
            }
            else
            {
                GameManager.Instance.EventManager.OnRaiseWarning-=RaiseWarning;
            }
        }

        private void RaiseWarning()
        {
            Instantiate(warningObject, transform);
        }
    }
}