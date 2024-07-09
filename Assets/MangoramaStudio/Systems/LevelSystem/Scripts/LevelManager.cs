using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Controllers;
using MangoramaStudio.Scripts.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{
    public class LevelManager : BaseManager
    {
        [SerializeField] private int _totalLevelCount;
        private LevelBehaviour _currentLevel;

        #region Initialize

        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);

            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            GameManager.EventManager.OnStartGame += StartGame;

            SROptions.OnLevelInvoked += RetryCurrentLevel;

        }

        private void OnDestroy()
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

            _currentLevel.Initialize(GameManager);

            GameManager.AddressableManager.SetPreLoadedLevelBehaviour();
            if (PlayerData.CurrentLevelId < _totalLevelCount)
            {
                GameManager.AddressableManager.LoadLevelAsync($"Level_{PlayerData.CurrentLevelId + 1}");
            }
        }

        private LevelBehaviour _desiredLoadedLevelPrefab;

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
