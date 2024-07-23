using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.VibrationModule;
using UnityEngine;

namespace MangoramaStudio.Systems.VibrationSystem
{
    public class VibrationManager : BaseManager
    {

        public void TryVibrate(VibrationType preset = VibrationType.Light)
        {
            if (PlayerData.IsHapticsEnabled == 1)
            {
                Vibrator.Vibrate(preset);     
            }
        }

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
    }
}