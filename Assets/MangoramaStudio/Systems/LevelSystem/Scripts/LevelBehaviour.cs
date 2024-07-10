using System;
using Behaviours;
using UnityEngine;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Controllers;
using Mechanics.Scripts;
using Sirenix.OdinInspector;

namespace MangoramaStudio.Scripts.Behaviours
{


    public class LevelBehaviour : MonoBehaviour
    {

        [SerializeField] private PlayableMechanicContainer container;
        private GameManager _gameManager;

        private bool _isLevelEnded;

        public void Initialize( bool isRestart = false)
        {
            _gameManager = GameManager.Instance;

            if (!isRestart)
            {
                _gameManager.EventManager.StartLevel();
            }
            
           container.Playable.Enable();
           container.Playable.Initialize();
           container.Playable.Prepare();
        }

        private void OnDestroy()
        {
            container.Playable.Disable();
            container.Playable.Dispose();
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