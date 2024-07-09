using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        [SerializeField] private SettingsPanel _settingsPanel;

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
        }

        public void ToggleSettingsPanel()
        {
            _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeSelf);
            _settingsPanel.InitializeContainers();
        }

        private void OnDestroy()
        {
            
        }
    }
}