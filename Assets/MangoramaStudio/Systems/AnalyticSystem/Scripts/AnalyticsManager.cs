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

            GameManager.EventManager.OnLevelStarted += LevelStartedNotification;
            GameManager.EventManager.OnLevelFinished += LevelFinishedNotification;
        }

        private void OnDestroy()
        {
            GameManager.EventManager.OnLevelStarted -= LevelStartedNotification;
            GameManager.EventManager.OnLevelFinished -= LevelFinishedNotification;
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
            TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Started");

        }

        private void LevelFinishedNotification(bool isSuccess)
        {
            if (isSuccess)
                TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Completed");
            else
                TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Failed");
        }



    }
}