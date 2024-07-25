using System;
using System.Collections;
using System.Collections.Generic;
using MangoramaStudio.Systems.PopupSystem.Scripts;
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

        public event Action OnAutoComplete;
        
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

        public void AutoComplete()
        {
            OnAutoComplete?.Invoke();   
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

        #region Popup Events

        public event Action<PopupType> OnOpenPopup;

        public event Action<PopupType> OnHidePopup;

        public void OpenPopup(PopupType popupType)
        {
            OnOpenPopup?.Invoke(popupType);
        }
        
        public void HidePopup(PopupType popupType)
        {
            OnHidePopup?.Invoke(popupType);
        }
        

        #endregion

        #region Data Events

        public event Action OnSaveData;

        public void SaveData()
        {
            OnSaveData?.Invoke();
        }


        #endregion

        #region Sounds Events

        public event Action OnPlayNotes;
        public event Action OnIncrementNoteIndex;
        public event Action OnDecrementNoteIndex;
        public event Action OnResetNoteIndex;
        public event Action OnPlayLevelSuccess;
        public event Action OnPlayFlowSuccess;
        public void PlayNoteSound()
        {
            OnPlayNotes?.Invoke();
        }
        public void IncrementNoteIndexSound()
        {
            OnIncrementNoteIndex?.Invoke();
        }
        public void DecrementNoteIndexSound()
        {
            OnDecrementNoteIndex?.Invoke();
        }
        public void ResetNoteIndexSound()
        {
            OnResetNoteIndex?.Invoke();
        }

        public void PlayLevelSuccessSound()
        {
            OnPlayLevelSuccess?.Invoke();
        }

        public void PlayFlowSuccessSound()
        {
            OnPlayFlowSuccess?.Invoke();
        }
        
        
        #endregion

        #region Analytics Events

        public event Action<string, bool> OnSendFirebaseEvent;
        
        public void SendFirebaseEvent(string eventName, bool withParameter)
        {
            OnSendFirebaseEvent?.Invoke(eventName, withParameter);
        }
        

        #endregion
    }
}