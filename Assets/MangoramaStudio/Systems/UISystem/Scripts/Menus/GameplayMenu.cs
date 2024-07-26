using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using TMPro;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class GameplayMenu : BaseMenu
    {
        [SerializeField] private GameObject warningObject;
        [SerializeField] private PipeCompleteCounterUIController pipeCompleteCounterUIController;
        [SerializeField] private MoveCounterUIController moveCounterUIController;
        [SerializeField] private LevelCounterUIController levelCounterUIController;

        public int GetMoveCount() => moveCounterUIController.MoveCounter().MoveCount();
        
        public override void Initialize()
        {
            base.Initialize();
            pipeCompleteCounterUIController.Initialize();
            moveCounterUIController.Initialize();
            levelCounterUIController.Initialize();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                GameManager.Instance.EventManager.OnRaiseWarning+=RaiseWarning;
                
            }
            else
            {
                GameManager.Instance.EventManager.OnRaiseWarning-=RaiseWarning;
            }
        }

        private void RaiseWarning()
        {
            if (warningObject == null)
            {
                Debug.LogError("WarningObject is null");
                return;
            }
            Instantiate(warningObject, transform);
        }
    }
}