using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Data;
using MatchinghamGames.GameUtilities.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public LevelBehaviour PreLoadedLevelBehaviour => _preLoadedLevelBehaviour;
        private LevelBehaviour _preLoadedLevelBehaviour;


        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            var prefix = "Level";
            LoadLevelAsync($"{prefix}_{PlayerData.CurrentLevelId}", () =>
            {
                FirstLevelLoaded?.Invoke();
            });
        }

        public void LoadLevelAsync(string levelName, Action onComplete = null)
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
                AfterFirstLevelLoad(onComplete);
            };
        }

        private async void AfterFirstLevelLoad(Action action)
        {
            await Task.Delay(5000);
            action?.Invoke();
        }

        public void SetPreLoadedLevelBehaviour()
        {
            _preLoadedLevelBehaviour = _loadedLevelBehaviour;
        }


    }

}