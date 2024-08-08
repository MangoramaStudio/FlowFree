using System.Collections.Generic;
using DG.Tweening;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using Mechanics.RoboticFlows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class GameplayMenu : BaseMenu
    {
        [SerializeField] private TextMeshProUGUI tutorialTMP;
        [SerializeField] private GameObject warningObject;
        [SerializeField] private PipeCompleteCounterUIController pipeCompleteCounterUIController;
        [SerializeField] private MoveCounterUIController moveCounterUIController;
        [SerializeField] private LevelCounterUIController levelCounterUIController;

        [SerializeField] private List<LevelTypeMenuDefinition> levelTypeMenuDefinitions = new();
        [SerializeField] private Image topBg, bottomBg;
        public List<LevelTypeMenuDefinition> LevelTypeMenuDefinitions => levelTypeMenuDefinitions;
        public int GetMoveCount() => moveCounterUIController.MoveCounter().MoveCount();
        
        public override void Initialize()
        {
            base.Initialize();
            pipeCompleteCounterUIController.Initialize();
            moveCounterUIController.Initialize();
            levelCounterUIController.Initialize();
            var definition = levelTypeMenuDefinitions.Find(x =>
                x.levelType == GameManager.Instance.LevelManager.CurrentLevel.LevelType);
            topBg.sprite = definition.topHeader;
            bottomBg.sprite = definition.bottomHeader;
            
            tutorialTMP.gameObject.SetActive(GameManager.Instance.LevelManager.CurrentLevel.IsTutorial);
            
            
        }
        
        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                GameManager.Instance.EventManager.OnRaiseWarning+=RaiseWarning;
                GameManager.Instance.EventManager.OnTutorialPlaying+=TutorialPlaying;
            }
            else
            {
                GameManager.Instance.EventManager.OnRaiseWarning-=RaiseWarning;
                GameManager.Instance.EventManager.OnTutorialPlaying-=TutorialPlaying;
            }
        }

        private void TutorialPlaying(string text)
        {
            tutorialTMP.SetText(text);         
        }

        private void RaiseWarning()
        {
            /*
            if (warningObject == null)
            {
                Debug.LogError("WarningObject is null");
                return;
            }
            Instantiate(warningObject, transform);
            */
            GameManager.Instance.EventManager.OpenPopup(PopupType.CoverAllTiles);
        }
    }
}