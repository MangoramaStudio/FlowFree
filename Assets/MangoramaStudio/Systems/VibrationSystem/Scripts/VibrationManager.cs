using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.VibrationModule;

namespace MangoramaStudio.Systems.VibrationSystem
{
    public class VibrationManager : BaseManager
    {
        private SettingsData _settingsData;

        public override void Initialize()
        {
            base.Initialize();
            _settingsData = GameManager.Instance.DataManager.GetData<SettingsData>();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.EventManager;
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                eventManager.OnVibrateDrawCell += VibrateDrawLine;
                eventManager.OnVibrateFlowComplete += VibrateLineComplete;
                eventManager.OnVibrateLevelComplete += VibrateLevelComplete;
                eventManager.OnVibrateDrawCellNode += VibrateDrawCellNode;
            }
            else
            {
                eventManager.OnVibrateDrawCell -= VibrateDrawLine;
                eventManager.OnVibrateFlowComplete -= VibrateLineComplete;
                eventManager.OnVibrateLevelComplete -= VibrateLevelComplete;
                eventManager.OnVibrateDrawCellNode -= VibrateDrawCellNode;

            }
        }

        private void VibrateDrawCellNode()
        {
            TryVibrate(VibrationType.Rigid);
        }

        private void VibrateDrawLine()
        {
            TryVibrate(VibrationType.Medium);
        }

        private void VibrateLineComplete()
        {
            TryVibrate(VibrationType.Medium);
        }

        private void VibrateLevelComplete()
        {
            TryVibrate(VibrationType.Heavy);
        }

        public void VibrateButton(VibrationType type = VibrationType.Light)
        {
            TryVibrate(type);
        }
        
        private void TryVibrate(VibrationType preset = VibrationType.Light)
        {
            if (_settingsData.isHapticEnabled == 1)
            {
                Vibrator.Vibrate(preset);     
            }
        }

    }
}