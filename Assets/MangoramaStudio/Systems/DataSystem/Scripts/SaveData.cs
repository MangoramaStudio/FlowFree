using System;

namespace MangoramaStudio.Scripts.Data
{
    [Serializable]
    public class SaveData : Data
    {
        public LevelData levelData;
        public TutorialData tutorialData;
        public SettingsData settingsData;
    }


    [Serializable]
    public class LevelData : Data
    {
        public int currentLevelIndex;
    }
    
    [Serializable]
    public class TutorialData : Data
    {
        public bool firstLevelShown;
    }

    [Serializable]
    public class SettingsData : Data
    {
        public int isMusicEnabled = 1;
        public int isHapticEnabled = 1;
        public int isSfxEnabled = 1;
    }
    
    public class Data 
    {
       
    }
}