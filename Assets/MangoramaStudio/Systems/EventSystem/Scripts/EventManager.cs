using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{

    public class EventManager : BaseManager
    {
        #region Common Events
        public event Action OnStartGame;
        public event Action<bool> OnLevelFinished;
        public event Action OnLevelStarted;
        
        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void StartLevel()
        {
            OnLevelStarted?.Invoke();
        }
        
        public void LevelFailed()
        {
            OnLevelFinished?.Invoke(false);
        }

        public void LevelCompleted()
        {
            OnLevelFinished?.Invoke(true);
        }
        
        #endregion

        #region Flow Drawer Events

        public event Action OnRaiseWarning;
        public event Action OnRaiseHint;
        public event Action OnDrawCell;
        public event Action OnCompleteFlow;
        public event Action OnCompleteAllFlows;
        public event Action OnRestartLevel;

        public event Action OnUndo;
        
        public void RaiseWarning()
        {
            OnRaiseWarning?.Invoke();
        }

        public void RaiseHint()
        {
            OnRaiseHint?.Invoke();
        }

        public void DrawCell()
        {
            OnDrawCell?.Invoke();
        }

        public void CompleteFlow()
        {
            OnCompleteFlow?.Invoke();
        }

        public void CompleteAllFlows()
        {
            OnCompleteAllFlows?.Invoke();
        }

        public void RestartLevel()
        {
            OnRestartLevel?.Invoke();
        }

        public void Undo()
        {
            OnUndo?.Invoke();
        }



        #endregion

        #region Ads Events

        public event Action<string> OnShowInterstitial;

        public event Action<Action, Action, string> OnShowRewarded;

        public void ShowInterstitial(string adTag)
        {
            OnShowInterstitial?.Invoke(adTag);
        }

        public void ShowRewarded(Action onRewardedSuccess, Action onRewardedFailure, string adTag)
        {
            OnShowRewarded?.Invoke(onRewardedSuccess, onRewardedFailure, adTag);
        }

        #endregion
    }
}