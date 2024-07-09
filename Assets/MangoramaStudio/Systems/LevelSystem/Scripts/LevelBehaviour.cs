using UnityEngine;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Controllers;
using Sirenix.OdinInspector;

namespace MangoramaStudio.Scripts.Behaviours
{


    public class LevelBehaviour : MonoBehaviour
    {
        public float WinPanelDelayTime => _winPanelDelayTime;
        public float LosePanelDelayTime => _losePanelDelayTime;

        [SerializeField] private float _winPanelDelayTime;
        [SerializeField] private float _losePanelDelayTime;

        private GameManager _gameManager;

        private bool _isLevelEnded;

        public void Initialize( bool isRestart = false)
        {
            _gameManager = GameManager.Instance;

            if (!isRestart)
            {
                _gameManager.EventManager.StartLevel();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown("c"))
            {
                LevelCompleted();
            }

            if (Input.GetKeyDown("f"))
            {
                LevelFailed();
            }
        }

        private void OnDestroy()
        {

        }

        [Button]
        private void LevelCompleted()
        {
            if (_isLevelEnded) return;

            _gameManager.EventManager.LevelCompleted();
            InputController.IsInputDeactivated = true;
            _isLevelEnded = true;
        }

        private void LevelFailed()
        {
            if (_isLevelEnded) return;

            _gameManager.EventManager.LevelFailed();
            InputController.IsInputDeactivated = true;
            _isLevelEnded = true;
        }
    }

}