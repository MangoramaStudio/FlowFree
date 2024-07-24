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

        
        private SettingsData _settingsData;

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
            _settingsData = GameManager.Instance.DataManager.GetData<SettingsData>();
        }

        public void InitializeContainers()
        {
           
            musicSettingsContainer.Initialize(_settingsData.isMusicEnabled == 1);
            sfxSettingsContainer.Initialize(_settingsData.isSfxEnabled == 1);
            vibrateSettingsContainer.Initialize(_settingsData.isHapticEnabled == 1);
            
            GameManager.Instance.EventManager.SaveData();
        }

        public void ToggleMusic()
        {
            if (_settingsData.isMusicEnabled == 1)
            {
                _settingsData.isMusicEnabled = 0;
            }
            else
            {
                _settingsData.isMusicEnabled = 1;
            }

            InitializeContainers();
        }

        public void ToggleSfx()
        {
            if (_settingsData.isSfxEnabled == 1)
            {
                _settingsData.isSfxEnabled = 0;
            }
            else
            {
                _settingsData.isSfxEnabled = 1;
            }

            InitializeContainers();
        }

        public void ToggleHaptic()
        {
            if (_settingsData.isHapticEnabled == 1)
            {
                _settingsData.isHapticEnabled = 0;
            }
            else
            {
                _settingsData.isHapticEnabled = 1;
            }

            InitializeContainers();
        }
    }

}