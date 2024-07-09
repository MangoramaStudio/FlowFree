using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Controllers;
using MangoramaStudio.Scripts.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Scripts.Managers
{
    public class LevelManager : BaseManager
    {
        [FormerlySerializedAs("_totalLevelCount")] [SerializeField] private int totalLevelCount;
        private LevelBehaviour _currentLevel;
        private LevelBehaviour _desiredLoadedLevelPrefab;

        #region Initialize

        public override void Initialize()
        {
            base.Initialize();

            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            GameManager.EventManager.OnStartGame += StartGame;

            SROptions.OnLevelInvoked += RetryCurrentLevel;

        }

        public override void OnDestroy()
        {
            GameManager.EventManager.OnStartGame -= StartGame;
            SROptions.OnLevelInvoked -= RetryCurrentLevel;
        }

        #endregion

        private void StartGame()
        {
            ClearLevel();

            Resources.UnloadUnusedAssets();

            InputController.IsInputDeactivated = false;

            _currentLevel = Instantiate(_desiredLoadedLevelPrefab);

            _currentLevel.Initialize();

            GameManager.AddressableManager.SetPreLoadedLevelBehaviour();
            if (PlayerData.CurrentLevelId < totalLevelCount)
            {
                GameManager.AddressableManager.LoadLevelAsync($"Level_{PlayerData.CurrentLevelId + 1}");
            }
        }

       

        private void ClearLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);
            }
        }

        [Button]
        public void ContinueToNextLevel() // For button
        {
            PlayerData.CurrentLevelId += 1;
            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            StartGame();
        }

        [Button]
        public void RetryCurrentLevel() // For button
        {
            _desiredLoadedLevelPrefab = GameManager.AddressableManager.PreLoadedLevelBehaviour;
            StartGame();

        }

    }
}
