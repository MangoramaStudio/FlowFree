using System;
using Behaviours;
using UnityEngine;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Controllers;
using Mechanics.RoboticFlows;
using Mechanics.Scripts;
using Sirenix.OdinInspector;

namespace MangoramaStudio.Scripts.Behaviours
{

    public enum LevelType
    {
        Default,
        Hard,
        SuperHard
    }

    public class LevelBehaviour : MonoBehaviour
    {

        [SerializeField] private LevelType levelType;
        [SerializeField] private FlowGrid flowGrid;
        [SerializeField] private PlayableMechanicContainer container;
        [SerializeField] private PipeCompleteCounter pipeCompleteCounter;
        [SerializeField] private MoveCounter moveCounter;
        public PlayableMechanicContainer Container => container;
        public PipeCompleteCounter PipeCompleteCounter => pipeCompleteCounter;
        public MoveCounter MoveCounter => moveCounter;

        public LevelType LevelType => levelType;
        public bool HasObstacles => flowGrid.HasObstacles;
        
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