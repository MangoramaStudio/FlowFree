using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.VibrationModule;

namespace MangoramaStudio.Systems.VibrationSystem
{
    public class VibrationManager : BaseManager
    {

        public void VibrateDrawLine()
        {
            TryVibrate();
        }

        public void VibrateLineComplete()
        {
            TryVibrate(VibrationType.Medium);
        }

        public void VibrateLevelComplete()
        {
            TryVibrate(VibrationType.Heavy);
        }

        public void VibrateButton(VibrationType type = VibrationType.Light)
        {
            TryVibrate(type);
        }
        
        private void TryVibrate(VibrationType preset = VibrationType.Light)
        {
            if (PlayerData.IsHapticsEnabled == 1)
            {
                Vibrator.Vibrate(preset);     
            }
        }

    }
}