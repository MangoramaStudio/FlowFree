using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using TMPro;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class GameplayMenu : BaseMenu
    {
        [SerializeField] private GameObject warningObject;
        [SerializeField] private TextMeshProUGUI pipeCounterTMP;

        private PipeCompleteCounter _pipeCompleteCounter;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetPipeCompleteCounter();
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
            
            GatherPipeCompleteData(isToggled);
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

        private void SetPipeCompleteCounter()
        {
            if (GameManager.Instance.LevelManager.CurrentLevel == null)
            {
                Debug.LogError("Current level is null");
                return;
            }
            _pipeCompleteCounter = GameManager.Instance.LevelManager.CurrentLevel.PipeCompleteCounter;
        }

        private void GatherPipeCompleteData(bool isToggled)
        {
            if (_pipeCompleteCounter == null)
            {
                Debug.LogError("PipeCompleteCounter is null");
                return;
            }
            
            if (isToggled)
            {
                _pipeCompleteCounter.onCompletePipe += UpdatePipCompleteTMP;
            }
            else
            {
                _pipeCompleteCounter.onCompletePipe -= UpdatePipCompleteTMP;
            }
        }

        private void UpdatePipCompleteTMP(int amount)
        {
            pipeCounterTMP.SetText($"%{amount}");
        }
    }
}