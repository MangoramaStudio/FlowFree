using System;
using System.Collections;
using System.Collections.Generic;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Scripts.UI
{
    public class SettingsPanel : UIPanel
    {
        [SerializeField] private SettingsContainer _musicSettingsContainer;
        [SerializeField] private SettingsContainer _sfxSettingsContainer;
        [SerializeField] private SettingsContainer _vibrateSettingsContainer;

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
        }

        public void InitializeContainers()
        {
            _musicSettingsContainer.Initialize(PlayerData.IsMusicEnabled == 1);
            _sfxSettingsContainer.Initialize(PlayerData.IsSfxEnabled == 1);
            _vibrateSettingsContainer.Initialize(PlayerData.IsHapticsEnabled == 1);
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