using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.CourtModule;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Systems.ReviewSystem.Scripts
{
    public class ReviewManager : BaseManager
    {
        [FormerlySerializedAs("_requestedLevel")] [SerializeField] private int requestedLevel;
        [FormerlySerializedAs("_isAppReviewActive")] [SerializeField] private bool isAppReviewActive;
        public override void Initialize()
        {
            if (isAppReviewActive)
            {
                GameManager.EventManager.OnLevelFinished += RequestCourt;
            }
        }

        private void RequestCourt(bool _)
        {
            if (levelData.currentLevelIndex == requestedLevel)
            {
                //Court.RequestJudgement(DismissAppReview);
                GameManager.EventManager.OnLevelFinished -= RequestCourt;
            }
        }

        private void DismissAppReview()
        {
            Debug.Log("review dismissed");
        }

    }
}