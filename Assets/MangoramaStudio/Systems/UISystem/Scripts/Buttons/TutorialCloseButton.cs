using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Buttons
{
    public class TutorialCloseButton : BaseButton
    {
        
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            IsClicked = true;
            GameManager.Instance.EventManager.HidePopup(PopupType.Tutorial);
        }
        
    }
}