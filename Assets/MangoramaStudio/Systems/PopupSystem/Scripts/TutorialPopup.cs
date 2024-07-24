using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    public class TutorialPopup : PopupBase
    {

        private TutorialData _tutorialData;

        protected override void Start()
        {
            base.Start();
            _tutorialData = GameManager.Instance.DataManager.GetData<TutorialData>();
        }

        public override void Hide()
        {
            base.Hide();
            _tutorialData.firstLevelShown = true;
            GameManager.Instance.EventManager.SaveData();
        }
    }
}