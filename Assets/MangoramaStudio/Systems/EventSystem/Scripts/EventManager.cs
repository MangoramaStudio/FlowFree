using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{

    public class EventManager : BaseManager
    {
        public event Action OnStartGame;
        public event Action<bool> OnLevelFinished;
        public event Action OnLevelStarted;
        public event Action OnRaiseWarning;
        public event Action OnRaiseHint;
        
        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void StartLevel()
        {
            OnLevelStarted?.Invoke();
        }

        public void RaiseWarning()
        {
            OnRaiseWarning?.Invoke();
        }

        public void RaiseHint()
        {
            OnRaiseHint?.Invoke();
        }

     

        public void LevelFailed()
        {
            OnLevelFinished?.Invoke(false);
        }

        public void LevelCompleted()
        {
            OnLevelFinished?.Invoke(true);
        }
    }
}