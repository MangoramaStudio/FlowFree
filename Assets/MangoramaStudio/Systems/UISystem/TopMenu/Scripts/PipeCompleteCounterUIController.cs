using System.Collections.Generic;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.RoboticFlows
{
    public class PipeCompleteCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI pipeCounterTMP,pipeCounterHeaderTMP;
        [SerializeField] private Image bg, headerBg;
        private PipeCompleteCounter _pipeCompleteCounter;
        private EventManager _eventManager;

        public void Initialize()
        {
            _eventManager = GameManager.Instance.EventManager;
           SetPipeCompleteCounter();
           UpdatePipeCompleteTMP(0);
           GatherPipeCompleteData(false);
           GatherPipeCompleteData(true);
           SetTheme(GameManager.Instance.LevelManager.CurrentLevel.LevelType);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GatherPipeCompleteData(false);
        }

        private void SetTheme(LevelType levelType)
        {
            var definition = GameManager.Instance.UIManager.GameplayMenu().LevelTypeMenuDefinitions.Find(x => x.levelType == levelType);
            bg.sprite = definition.topMenuPipeBg;
            headerBg.sprite = definition.topMenuPipeHeaderBg;
            pipeCounterTMP.color = definition.counterColor;
            pipeCounterHeaderTMP.fontSharedMaterial = definition.headerTMPMaterial;

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
            pipeCounterTMP.SetText($"{amount}%");
        }

        private void Restart()
        {
            _pipeCompleteCounter.Restart();
            UpdatePipeCompleteTMP(0);   
        }
        
    }
}