using System;
using System.Collections;
using System.Collections.Generic;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using Mechanics.RoboticFlows;
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
        public event Action<FlowDrawer> OnDrawCell;
        public event Action<FlowDrawer> OnSelectOccupiedCell; 
        public event Action<FlowDrawer> OnReleaseDrawing; 
        public event Action<FlowDrawer,Node> OnCompleteFlow;
        public event Action OnCompleteAllFlows;
        public event Action OnRestartLevel;
        public event Action OnUndo;
        public event Action OnAutoComplete;
        public event Action<FlowDrawer> OnClearCell;
        public event Action<FlowDrawer> OnClearDisconnectedCell;
        public event Action<FlowDrawer> OnResetFlow;
        public event Action<FlowDrawer> OnConnectFlow; 

        public void ReleaseDrawing(FlowDrawer flowDrawer)
        {
            OnReleaseDrawing?.Invoke(flowDrawer);
        }
        
        public void RaiseWarning()
        {
            OnRaiseWarning?.Invoke();
        }

        public void RaiseHint()
        {
            OnRaiseHint?.Invoke();
        }

        public void DrawCell(FlowDrawer drawer)
        {
            OnDrawCell?.Invoke(drawer);
        }

        public void SelectOccupiedCell(FlowDrawer drawer)
        {
            OnSelectOccupiedCell?.Invoke(drawer);
        }

        public void ConnectFlow(FlowDrawer flowDrawer)
        {
            OnConnectFlow?.Invoke(flowDrawer);
        }

        public void ResetFlow(FlowDrawer flowDrawer)
        {
            OnResetFlow?.Invoke(flowDrawer);
        }

        public void ClearCell(FlowDrawer flowDrawer)
        {
            OnClearCell?.Invoke(flowDrawer);
        }

        public void ClearDisconnectedCell(FlowDrawer flowDrawer)
        {
            //OnClearDisconnectedCell?.Invoke(flowDrawer);
        }

        public void CompleteFlow(FlowDrawer flowDrawer,Node selectedNode)
        {
            OnCompleteFlow?.Invoke(flowDrawer,selectedNode);
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

        public event Action<Action, Action,Action, string> OnShowRewarded;

        public void ShowInterstitial(string adTag)
        {
            OnShowInterstitial?.Invoke(adTag);
        }

        public void ShowRewarded(Action onRewardedSuccess, Action onRewardedFailure,Action onAdNotReady ,string adTag)
        {
            OnShowRewarded?.Invoke(onRewardedSuccess, onRewardedFailure,onAdNotReady, adTag);
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
        public event Action<int> OnDecrementNoteIndex;
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
        public void DecrementNoteIndexSound(int index)
        {
            OnDecrementNoteIndex?.Invoke(index);
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

        #region Vibration Events

        public event Action OnVibrateFlowComplete;
        public event Action OnVibrateLevelComplete;
        public event Action OnVibrateDrawCell;
        public event Action OnVibrateDrawCellNode;

        public void VibrateFlowComplete()
        {
            OnVibrateFlowComplete?.Invoke();
        }
        
        public void VibrateLevelComplete()
        {
            OnVibrateLevelComplete?.Invoke();
        }

        public void VibrateDrawCell()
        {
            OnVibrateDrawCell?.Invoke();
        }
        
        public void VibrateDrawCellNode()
        {
            OnVibrateDrawCellNode?.Invoke();
        }

        #endregion

        #region Tutorial Events

        public event Action<string> OnTutorialPlaying;
        
        public event Action OnTutorialCompleted;

        public void PlayTutorial(string text)
        {
            OnTutorialPlaying?.Invoke(text);
        }

        public void CompleteTutorial()
        {
            OnTutorialCompleted?.Invoke();
        }
        
        #endregion
    }
}