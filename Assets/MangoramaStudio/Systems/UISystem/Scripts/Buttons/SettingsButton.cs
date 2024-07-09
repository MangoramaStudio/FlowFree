using MangoramaStudio.Scripts.UI;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class SettingsButton : BaseButton
    {
        [SerializeField] private SettingsPanel settingsPanel;
        public override void Click()
        {
            base.Click();
            settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
            settingsPanel.InitializeContainers();
        }
    }
}