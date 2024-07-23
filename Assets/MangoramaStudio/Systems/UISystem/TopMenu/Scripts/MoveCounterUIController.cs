using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class MoveCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveCounterTMP;
        private MoveCounter _moveCounter;
        
        public void Initialize()
        {
            SetMoveCounter();
            UpdateMoveCounterTMP(0);
            GatherMoveCounterData(false);
            GatherMoveCounterData(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GatherMoveCounterData(false);
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
                GameManager.Instance.EventManager.OnRestartLevel += Restart;
            }
            else
            {
                _moveCounter.onMoveCountUpdate -= UpdateMoveCounterTMP;
                GameManager.Instance.EventManager.OnRestartLevel -= Restart;
            }
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