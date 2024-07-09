using MangoramaStudio.Scripts.Data;
using MatchinghamGames.CourtModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{

    public class AppReviewManager : MonoBehaviour
    {
        [SerializeField] private bool _isAppReviewActive;
        [SerializeField] private int _requestedLevel;

        public void Initialize()
        {
            if (_isAppReviewActive)
            {
                GameManager.Instance.EventManager.OnLevelFinished += RequestCourt;
            }
        }

        private void RequestCourt(bool _)
        {
            if (PlayerData.CurrentLevelId == _requestedLevel)
            {
                Court.RequestJudgement(DismissAppReview);
                GameManager.Instance.EventManager.OnLevelFinished -= RequestCourt;
            }
        }

        private void DismissAppReview()
        {
            Debug.Log("review dismissied");
        }
    }
}