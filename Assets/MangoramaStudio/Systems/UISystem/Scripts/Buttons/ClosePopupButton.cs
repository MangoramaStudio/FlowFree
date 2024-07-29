using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Buttons
{
    public class ClosePopupButton : BaseButton
    {
        [SerializeField] private PopupType popupType;
        
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            
            IsClicked = true;
            GameManager.Instance.EventManager.HidePopup(popupType);
        }
        
    }
}