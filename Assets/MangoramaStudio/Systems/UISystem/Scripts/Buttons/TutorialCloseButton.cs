using MangoramaStudio.Scripts.Managers.Buttons;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Buttons
{
    public class TutorialCloseButton : BaseButton
    {

        [SerializeField] private PopupBase popupBase;
        
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            IsClicked = true;
            popupBase.Hide();
        }
        
    }
}