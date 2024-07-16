using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Data;
using MatchinghamGames.GameUtilities.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangoramaStudio.Systems.LevelSystem.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MangoramaStudio.Scripts.Managers
{

    public class AddressableManager : MonoBehaviour
    {
        public event Action FirstLevelLoaded;
        public LevelBehaviour LoadedLevelBehaviour => _loadedLevelBehaviour;
        private LevelBehaviour _loadedLevelBehaviour;
       
        private LevelOrderHandler LevelOrderHandler =>
            _levelOrderHandler ? _levelOrderHandler : (_levelOrderHandler = GetComponent<LevelOrderHandler>());
        private LevelOrderHandler _levelOrderHandler;
        
        private const string Prefix = "Level";
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            LevelOrderHandler.Initialize();
            
            LoadLevelAsync(GetCurrentLevel(), () =>
            {
                FirstLevelLoaded?.Invoke();
            });
        }
        
        public void LoadCurrentLevelAsync(Action onComplete = null)
        {
            LoadLevelAsync(GetCurrentLevel(),onComplete);
        }

        private void LoadLevelAsync(string levelName, Action onComplete = null)
        {
            var target = Addressables.LoadAssetAsync<GameObject>(levelName);

            target.Completed += asyncOperationHandle =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedLevelBehaviour = asyncOperationHandle.Result.GetComponent<LevelBehaviour>();
                }
                else
                {
                    Debug.LogError(asyncOperationHandle.Status);
                }
                OnLoadComplete(onComplete);
            };
        }

   
        private void OnLoadComplete(Action action)
        {
            action?.Invoke();
        }
        
        private string GetCurrentLevel()
        {
            var list = LevelOrderHandler.GetCurrentLevelOrder();
            var element = list.ElementAt(PlayerData.CurrentLevelId);
            return $"{Prefix} {element}";
        }
        
    }

}