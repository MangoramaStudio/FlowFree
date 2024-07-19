using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class PipeCompleteCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI pipeCounterTMP;

        private PipeCompleteCounter _pipeCompleteCounter;

        public void Initialize()
        {
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