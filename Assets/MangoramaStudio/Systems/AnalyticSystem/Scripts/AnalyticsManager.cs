using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using MatchinghamGames.SherlockModule;
using MatchinghamGames.SherlockModule.Services.Firebase;
using MatchinghamGames.SherlockModule.Services.Adjust;


namespace MangoramaStudio.Scripts.Managers
{
    public class AnalyticsManager : BaseManager
    {
        public override void Initialize()
        {
            base.Initialize();
            InitializeFacebook();
        }
        
        protected override void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.EventManager;
            if (isToggled)
            {
                eventManager.OnLevelStarted += LevelStartedNotification;
                eventManager.OnLevelFinished += LevelFinishedNotification;
                eventManager.OnRaiseHint += HintUseTrackEvent;
                eventManager.OnSendFirebaseEvent+=SendFirebaseEvent;;
            }
            else
            {
                eventManager.OnLevelStarted -= LevelStartedNotification;
                eventManager.OnLevelFinished -= LevelFinishedNotification;
                eventManager.OnRaiseHint -= HintUseTrackEvent;
                eventManager.OnSendFirebaseEvent -=SendFirebaseEvent;;
            }
        }

        private void SendFirebaseEvent(string eventName, bool withParameter)
        {
            var trackEventName = $"level{levelData.currentLevelIndex}_{eventName}";
            TrackEventFirebase(trackEventName,withParameter);    
        }
        

        private void HintUseTrackEvent()
        {
            var eventName = $"level{levelData.currentLevelIndex}_hint";
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

        private void TrackEventFirebase(string eventName, bool isWithParameter = false)
        {
            if (isWithParameter)
            {
                Sherlock.Service<IFirebaseAnalyticsService>().SendCustom(eventName, new Dictionary<string, object>
            {
                {"Level", levelData.currentLevelIndex }
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
            var eventName = $"level{levelData.currentLevelIndex}_start";
            TrackEventFirebase(eventName);
            TrackEventAdjust(eventName);

        }

        private void LevelFinishedNotification(bool isSuccess)
        {
            var successEventName = $"level{levelData.currentLevelIndex}_complete";
            if (isSuccess)
            {
                TrackEventFirebase(successEventName);
                TrackEventAdjust(successEventName);
            }
        }
    }
}