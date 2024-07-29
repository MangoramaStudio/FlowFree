using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.RoboticFlows
{
    public class MoveCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveCounterTMP;
        [SerializeField] private Image bg, headerBg;

        private MoveCounter _moveCounter;
        private EventManager _eventManager;

        public MoveCounter MoveCounter() => _moveCounter;
        
        public void Initialize()
        {
            _eventManager = GameManager.Instance.EventManager;
            SetMoveCounter();
            UpdateMoveCounterTMP(0);
            GatherMoveCounterData(false);
            GatherMoveCounterData(true);
            SetTheme(GameManager.Instance.LevelManager.CurrentLevel.LevelType);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GatherMoveCounterData(false);
        }
        
        private void SetTheme(LevelType levelType)
        {
            var definition = GameManager.Instance.UIManager.GameplayMenu().LevelTypeMenuDefinitions.Find(x => x.levelType == levelType);
            bg.sprite = definition.topMenuPipeBg;
            headerBg.sprite = definition.topMenuPipeHeaderBg;
            moveCounterTMP.color = definition.counterColor;
        }


        private void SetMoveCounter()
        {
            if (GameManager.Instance.LevelManager.CurrentLevel == null)
            {
                Debug.LogError("Current level is null");
                return;
            }
            _moveCounter = GameManager.Instance.LevelManager.CurrentLevel.MoveCounter;
        }

       
        
        private void GatherMoveCounterData(bool isToggled)
        {
            if (_moveCounter == null)
            {
                Debug.LogError("MoveCounter is null");
                return;
            }
            
            if (isToggled)
            {
                _moveCounter.onMoveCountUpdate += UpdateMoveCounterTMP;
                _eventManager.OnRestartLevel += Restart;
                _eventManager.OnUndo += Undo;
            }
            else
            {
                _moveCounter.onMoveCountUpdate -= UpdateMoveCounterTMP;
                _eventManager.OnRestartLevel -= Restart;
                _eventManager.OnUndo -= Undo;
            }
        }

        private void Undo()
        {
            _moveCounter.Undo();
        }

        private void Restart()
        {
            _moveCounter.Restart();
            UpdateMoveCounterTMP(0);
        }


        private void UpdateMoveCounterTMP(int amount)
        {
            if (moveCounterTMP == null)
            {
                Debug.LogError("MoveCounterTMP is null");
                return;
            }
            moveCounterTMP.SetText($"{amount}");
        }
    }
    
    
}