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

        public override void Initialize()
        {
            base.Initialize();
            SetPipeCompleteCounter();
            GatherPipeCompleteData(false);
            UpdatePipeCompleteTMP(0);
            GatherPipeCompleteData(true);
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
                _pipeCompleteCounter.onCompletePipe += UpdatePipeCompleteTMP;
            }
            else
            {
                _pipeCompleteCounter.onCompletePipe -= UpdatePipeCompleteTMP;
            }
        }

        private void UpdatePipeCompleteTMP(int amount)
        {
            if (pipeCounterTMP == null)
            {
                Debug.LogError("PipeCounterTMP is null");
                return;
            }
            pipeCounterTMP.SetText($"%{amount}");
        }
    }
}