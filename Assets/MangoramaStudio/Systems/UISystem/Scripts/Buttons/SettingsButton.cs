using MangoramaStudio.Scripts.UI;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class SettingsButton : BaseButton
    {
        [SerializeField] private SettingsPanel settingsPanel;

        protected override void Click()
        {
            base.Click();
            settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
            settingsPanel.InitializeContainers();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                
            }
            else
            {
                settingsPanel.gameObject.SetActive(false);
            }
        }
    }
}