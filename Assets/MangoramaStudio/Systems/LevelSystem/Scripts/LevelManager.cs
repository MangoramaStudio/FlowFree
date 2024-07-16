using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Controllers;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Systems.LevelSystem.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Scripts.Managers
{
    public class LevelManager : BaseManager
    {
        [FormerlySerializedAs("_totalLevelCount")] [SerializeField] private int totalLevelCount;
        
        private LevelBehaviour _currentLevel;
        private LevelBehaviour _desiredLoadedLevelPrefab;

  
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

        private void StartGame()
        {
            ClearLevel();
            Resources.UnloadUnusedAssets();
            InputController.IsInputDeactivated = false;
            if (PlayerData.CurrentLevelId < totalLevelCount)
            {
                GameManager.AddressableManager.LoadCurrentLevelAsync(OnLevelLoaded);
            }
        }
        
        private void OnLevelLoaded()
        {
            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            _currentLevel = Instantiate(_desiredLoadedLevelPrefab);
            _currentLevel.Initialize();        
        }
        
        private void ClearLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);
            }
        }

        public async void ContinueToNextLevel() 
        {
            IncrementLevel();
            await Task.Yield();
            GameManager.EventManager.StartLevel();
            StartGame();
        }

        private void RetryCurrentLevel()
        {
            GameManager.EventManager.StartLevel();
            StartGame();
        }

        private void IncrementLevel()
        {
            PlayerData.CurrentLevelId += 1;   
        }
    }
}
