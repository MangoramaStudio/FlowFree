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
            
            
            container.Initialize();
        }

        private void OnDestroy()
        {
            container.DeInitialize();
        }


        [Button]
        private void LevelCompleted()
        {
            if (_isLevelEnded) return;

            _gameManager.EventManager.LevelCompleted();
            InputController.IsInputDeactivated = true;
            _isLevelEnded = true;
            container.DeInitialize();
        }
        
    }

}