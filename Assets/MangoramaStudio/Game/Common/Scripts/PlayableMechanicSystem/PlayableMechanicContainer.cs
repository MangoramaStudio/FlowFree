using System;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using Mechanics.Scripts;
using UnityEngine;

namespace Behaviours
{
    public class PlayableMechanicContainer : MonoBehaviour
    {
        [SerializeField] private PlayableMechanicBehaviour playable;

        public PlayableMechanicBehaviour Playable => playable;

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
                GameManager.Instance.EventManager.OnRaiseHint += ShowHint;
            }
            else
            {
                Playable.Success -= GameManager.Instance.EventManager.LevelCompleted;
                Playable.Warn -= GameManager.Instance.EventManager.RaiseWarning;
                GameManager.Instance.EventManager.OnRaiseHint -= ShowHint;
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
        
        
    }
}