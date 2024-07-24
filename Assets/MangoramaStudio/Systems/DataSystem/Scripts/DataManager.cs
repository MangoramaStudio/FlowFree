using System.Collections.Generic;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.Handyman;
using Newtonsoft.Json;
using UnityEngine;

namespace MangoramaStudio.Scripts.Data
{
    public class DataManager : BaseManager
    {
        public const string SaveDataKey =  "SaveData";
   
        public SaveData SaveData => _saveData;
        private SaveData _saveData;
        
        private readonly List<Data> _dataList = new List<Data>();

        public override void Initialize()
        {
            base.Initialize();
            GetPlayerData();
            InitPlayerData(); 
        }

    
        
        public override void OnDestroy()
        {
            SavePlayerData();
            ToggleEvents(false);
        }

        private void InitPlayerData()
        {
            _saveData ??= new SaveData();
            _saveData.levelData ??= new LevelData();
            _saveData.tutorialData ??= new TutorialData();
            _saveData.settingsData??= new SettingsData();
            
            _dataList.Add(_saveData.levelData);
            _dataList.Add(_saveData.tutorialData);
            _dataList.Add(_saveData.settingsData);
            
            SavePlayerData();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {
                GameManager.Instance.EventManager.OnSaveData += SavePlayerData;
            }
            else
            {
                GameManager.Instance.EventManager.OnSaveData -= SavePlayerData;
            }
        }

        public T GetData<T>() where T : Data
        {
            return (T)_dataList.Find(x => x.GetType() == typeof(T));
        }
        
        private void Set<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        private T Get<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var objectToSave = JsonConvert.DeserializeObject<T>(json);
            return objectToSave;
        }
        
        private void GetPlayerData()
        {
            _saveData = Get<SaveData>(SaveDataKey);
            
#if UNITY_EDITOR
            Debug.Log($"SaveData is loaded: {JsonConvert.SerializeObject(SaveData,Formatting.Indented)}");
#endif
        }

        private void SavePlayerData()
        {
            Set(SaveDataKey,SaveData);
#if UNITY_EDITOR
            Debug.Log($"SaveData is saved: {JsonConvert.SerializeObject(SaveData,Formatting.Indented)}");
#endif

        }
        
    }
}