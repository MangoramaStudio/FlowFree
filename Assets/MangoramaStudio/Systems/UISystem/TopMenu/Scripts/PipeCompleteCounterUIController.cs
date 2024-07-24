using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class PipeCompleteCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI pipeCounterTMP;

        private PipeCompleteCounter _pipeCompleteCounter;
        private EventManager _eventManager;

        public void Initialize()
        {
            _eventManager = GameManager.Instance.EventManager;
           SetPipeCompleteCounter();
           UpdatePipeCompleteTMP(0);
           GatherPipeCompleteData(false);
           GatherPipeCompleteData(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GatherPipeCompleteData(false);
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
                _eventManager.OnRestartLevel += Restart;
                _eventManager.OnUndo += Undo;
                _eventManager.OnAutoComplete += AutoComplete;
            }
            else
            {
                _pipeCompleteCounter.onCompletePipe -= UpdatePipeCompleteTMP;
                _eventManager.OnRestartLevel -= Restart;
                _eventManager.OnUndo -= Undo;
                _eventManager.OnAutoComplete -= AutoComplete;
            }
        }

        private void AutoComplete()
        {
            _pipeCompleteCounter.AutoComplete();
        }

        private void Undo()
        {
            _pipeCompleteCounter.Undo();
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

        private void Restart()
        {
            _pipeCompleteCounter.Restart();
            UpdatePipeCompleteTMP(0);   
        }
        
    }
}