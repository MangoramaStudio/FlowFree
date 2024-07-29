using MangoramaStudio.Scripts.UI;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class SettingsButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            GameManager.Instance.EventManager.OpenPopup(PopupType.Settings);
        }
        
        
    }
}