using UnityEngine;

namespace MangoramaStudio.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        [SerializeField] private SettingsPanel _settingsPanel;
        
        public void ToggleSettingsPanel()
        {
            _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeSelf);
            _settingsPanel.InitializeContainers();
        }
        
    }
}