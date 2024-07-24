using System.Collections.Generic;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;
using MangoramaStudio.Scripts.Data;
using MatchinghamGames.SherlockModule;
using MatchinghamGames.SherlockModule.Services.Firebase;
using MatchinghamGames.SherlockModule.Services.GameAnalytics;
using MatchinghamGames.SherlockModule.Services.Adjust;


namespace MangoramaStudio.Scripts.Managers
{
    public class AnalyticsManager : BaseManager
    {
        public override void Initialize()
        {
            base.Initialize();
            InitializeFacebook();
            GameAnalytics.Initialize();
        }
        
        protected override void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.EventManager;
            if (isToggled)
            {
                eventManager.OnLevelStarted += LevelStartedNotification;
                eventManager.OnLevelFinished += LevelFinishedNotification;
                eventManager.OnRaiseHint += HintUseTrackEvent;
                eventManager.OnAutoComplete += SkipLevelTrackEvent;
            }
            else
            {
                eventManager.OnLevelStarted -= LevelStartedNotification;
                eventManager.OnLevelFinished -= LevelFinishedNotification;
                eventManager.OnRaiseHint -= HintUseTrackEvent;
                eventManager.OnAutoComplete -= SkipLevelTrackEvent;
            }
        }

        private void SkipLevelTrackEvent()
        {
            var eventName = $"Level_{PlayerData.CurrentLevelId}_SkipLevel_Used";
            TrackEventFirebase(eventName);
            TrackEventAdjust(eventName);
        }

        private void HintUseTrackEvent()
        {
            var eventName = $"Level_{PlayerData.CurrentLevelId}_Hint_Used";
            TrackEventFirebase(eventName);
            TrackEventAdjust(eventName);
        }

        #region FB_Initialize

        private void InitializeFacebook()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                FB.Init(FB.ActivateApp);
            }
        }
        #endregion

        #region Tracking_Events

        public void TrackEventAdjust(string eventName)
        {
            Debug.Log("adjust eventname : " + eventName);
            if (SherlockUtility.GetToken<IAdjustAnalyticsService>(eventName) == null || SherlockUtility.GetToken<IAdjustAnalyticsService>(eventName) == "")
            {
                return;
            }
            Sherlock.Service<IAdjustAnalyticsService>()
                .SendCustom(SherlockUtility.GetToken<IAdjustAnalyticsService>(eventName));
        }

        public void TrackEventGameAnalytics(string eventName, bool isWithParameter = false)
        {
            if (isWithParameter)
            {
                Sherlock.Service<IGameAnalyticsService>().SendCustom(eventName, new Dictionary<string, object>
            {
                {"Level", PlayerData.CurrentLevelId }
            });
            }
            else
            {
                Sherlock.Service<IGameAnalyticsService>().SendCustom(eventName);
            }

            Debug.Log($"GA {eventName}");
        }

        public void TrackEventFirebase(string eventName, bool isWithParameter = false)
        {
            if (isWithParameter)
            {
                Sherlock.Service<IFirebaseAnalyticsService>().SendCustom(eventName, new Dictionary<string, object>
            {
                {"Level", PlayerData.CurrentLevelId }
            });
            }
            else
            {
                Sherlock.Service<IFirebaseAnalyticsService>().SendCustom(eventName);
            }

            Debug.Log($"Firebase {eventName}");

        }
        #endregion

        private void LevelStartedNotification()
        {
            var eventName = $"Level_{PlayerData.CurrentLevelId}_Started";
            TrackEventFirebase(eventName);
            TrackEventAdjust(eventName);

        }

        private void LevelFinishedNotification(bool isSuccess)
        {
            var successEventName = $"Level_{PlayerData.CurrentLevelId}_Completed";
            var failedEventName = $"Level_{PlayerData.CurrentLevelId}_Failed";
            if (isSuccess)
            {
                TrackEventFirebase(successEventName);
                TrackEventAdjust(successEventName);
            }
             
            else
            {
                TrackEventFirebase(failedEventName); 
                TrackEventAdjust(failedEventName);
            }
        }
    }
}