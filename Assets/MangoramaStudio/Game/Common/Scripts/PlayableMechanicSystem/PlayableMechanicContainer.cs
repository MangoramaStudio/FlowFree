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
        [SerializeField] private RoboticFlowBuilder builder;
        public PlayableMechanicBehaviour Playable => playable;
        public RoboticFlowBuilder Builder => builder;

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

                Playable.Success += GameManager.Instance.EventManager.OpenWinMenu;
                Playable.Warn += GameManager.Instance.EventManager.RaiseWarning;
                EventManager.OnRaiseHint += ShowHint;
                EventManager.OnRestartLevel += Restart;
            }
            else
            {
                Playable.Success -= GameManager.Instance.EventManager.OpenWinMenu;
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


        private void Restart()
        { 
            Playable.RaiseRestart();
        }
        
    }
}