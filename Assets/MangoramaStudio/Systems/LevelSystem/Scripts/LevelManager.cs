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

        private LevelBehaviour _desiredLoadedLevelPrefab;


        public LevelBehaviour CurrentLevel { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            GameManager.EventManager.OnStartGame += StartGame;
         
        }

        public override void OnDestroy()
        {
            GameManager.EventManager.OnStartGame -= StartGame;
           
        }

        
        public void StartGameSR(int index)
        {
            ClearLevel();
            Resources.UnloadUnusedAssets();
            InputController.IsInputDeactivated = false;
            if (levelData.currentLevelIndex < totalLevelCount)
            {
                GameManager.AddressableManager.LoadCurrentLevelAsync(index,OnLevelLoaded);
            }
        }
        
        private void StartGame()
        {
            ClearLevel();
            Resources.UnloadUnusedAssets();
            InputController.IsInputDeactivated = false;
            if (levelData.currentLevelIndex < totalLevelCount)
            {
                GameManager.AddressableManager.LoadCurrentLevelAsync(levelData.currentLevelIndex,OnLevelLoaded);
            }
        }
        
        private void OnLevelLoaded()
        {
            _desiredLoadedLevelPrefab = GameManager.AddressableManager.LoadedLevelBehaviour;
            CurrentLevel = Instantiate(_desiredLoadedLevelPrefab);
            CurrentLevel.Initialize();        
        }
        
        private void ClearLevel()
        {
            if (CurrentLevel != null)
            {
                Destroy(CurrentLevel.gameObject);
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
            levelData.currentLevelIndex += 1;   
            GameManager.EventManager.SaveData();
        }
        
        
    }
}
