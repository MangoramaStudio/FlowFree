using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    public class TutorialPopup : PopupBase
    {

        private TutorialData _tutorialData;
        private LevelData _levelData;

        protected override void Start()
        {
            base.Start();
            _tutorialData = GameManager.Instance.DataManager.GetData<TutorialData>();
            _levelData = GameManager.Instance.DataManager.GetData<LevelData>();
        }

        public override void Hide()
        {
            var levelManager = GameManager.Instance.LevelManager;
            base.Hide();

            if (_levelData.currentLevelIndex == 0 && !_tutorialData.firstLevelShown)
            {
                _tutorialData.firstLevelShown = true;     
            }
            
            else if (levelManager.CurrentLevel.HasObstacles)
            {
                if (!_tutorialData.obstacleLevelShown)
                {
                    _tutorialData.obstacleLevelShown = true;
                }
            }
            GameManager.Instance.EventManager.SaveData();
        }
    }
}