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

            FB_Initialize();
            GameAnalytics.Initialize();

            GameManager.EventManager.OnLevelStarted += LevelStartedNotif;
            GameManager.EventManager.OnLevelFinished += LevelFinishedNotif;
        }

        private void OnDestroy()
        {
            GameManager.EventManager.OnLevelStarted -= LevelStartedNotif;
            GameManager.EventManager.OnLevelFinished -= LevelFinishedNotif;
        }

        #region FB_Initialize

        private void FB_Initialize()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => { FB.ActivateApp(); });
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

        private void LevelStartedNotif()
        {
            TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Started");

        }

        private void LevelFinishedNotif(bool isSuccess)
        {
            if (isSuccess)
                TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Completed");
            else
                TrackEventFirebase($"Level_{PlayerData.CurrentLevelId}_Failed");
        }



    }
}