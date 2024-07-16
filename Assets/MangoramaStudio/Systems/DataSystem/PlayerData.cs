using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MangoramaStudio.Scripts.Data
{
    public static class PlayerData
    {
        public static int CurrentLevelId
        {
            get => PlayerPrefs.GetInt("CurrentLevelId", 0); 

            set => PlayerPrefs.SetInt("CurrentLevelId", value);
        }

        public static int IsMusicEnabled
        {
            get => PlayerPrefs.GetInt("IsMusicEnabled", 1);

            set => PlayerPrefs.SetInt("IsMusicEnabled", value);
        }

        public static int IsHapticsEnabled
        {
            get => PlayerPrefs.GetInt("IsHapticsEnabled", 1);

            set => PlayerPrefs.SetInt("IsHapticsEnabled", value);
        }

        public static int IsSfxEnabled
        {
            get => PlayerPrefs.GetInt("IsSfxEnabled", 1);

            set => PlayerPrefs.SetInt("IsSfxEnabled", value);
        }
    }
}