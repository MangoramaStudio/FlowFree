using System;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using Mechanics.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviours
{
    public class PlayableMechanicContainer : MonoBehaviour
    {
        [SerializeField] private PlayableMechanicBehaviour playable;

        public PlayableMechanicBehaviour Playable => playable;

        private EventManager EventManager => GameManager.Instance.EventManager;
    

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {

                Playable.Success += GameManager.Instance.EventManager.LevelCompleted;
                Playable.Warn += GameManager.Instance.EventManager.RaiseWarning;
                EventManager.OnRaiseHint += ShowHint;
                EventManager.OnRestartLevel += Restart;
            }
            else
            {
                Playable.Success -= GameManager.Instance.EventManager.LevelCompleted;
                Playable.Warn -= GameManager.Instance.EventManager.RaiseWarning;
                EventManager.OnRaiseHint -= ShowHint; 
                EventManager.OnRestartLevel -= Restart;
            }
        }

        private void ShowHint()
        {
            Playable.GetComponent<RoboticFlowDrawer>().ShowHint();
        }

        public void Initialize()
        {
            Playable.Enable();
            Playable.Initialize();
            Playable.Prepare();
        }

        public void DeInitialize()
        {
            Playable.Disable();
            Playable.Dispose();
        }

        [Button]
        public void Restart()
        {
            Playable.Clear();
            Playable.Prepare();
        }
        
    }
}