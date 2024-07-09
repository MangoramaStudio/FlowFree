using System;
using System.Collections;
using System.Collections.Generic;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Scripts.UI
{
    public class SettingsPanel : UIPanel
    {
        [FormerlySerializedAs("_musicSettingsContainer")] [SerializeField] private SettingsContainer musicSettingsContainer;
        [FormerlySerializedAs("_sfxSettingsContainer")] [SerializeField] private SettingsContainer sfxSettingsContainer;
        [FormerlySerializedAs("_vibrateSettingsContainer")] [SerializeField] private SettingsContainer vibrateSettingsContainer;


        public void InitializeContainers()
        {
            musicSettingsContainer.Initialize(PlayerData.IsMusicEnabled == 1);
            sfxSettingsContainer.Initialize(PlayerData.IsSfxEnabled == 1);
            vibrateSettingsContainer.Initialize(PlayerData.IsHapticsEnabled == 1);
        }

        public void ToggleMusic()
        {
            if (PlayerData.IsMusicEnabled == 1)
            {
                PlayerData.IsMusicEnabled = 0;
            }
            else
            {
                PlayerData.IsMusicEnabled = 1;
            }

            InitializeContainers();
        }

        public void ToggleSfx()
        {
            if (PlayerData.IsSfxEnabled == 1)
            {
                PlayerData.IsSfxEnabled = 0;
            }
            else
            {
                PlayerData.IsSfxEnabled = 1;
            }

            InitializeContainers();
        }

        public void ToggleHaptic()
        {
            if (PlayerData.IsHapticsEnabled == 1)
            {
                PlayerData.IsHapticsEnabled = 0;
            }
            else
            {
                PlayerData.IsHapticsEnabled = 1;
            }

            InitializeContainers();
        }
    }

}