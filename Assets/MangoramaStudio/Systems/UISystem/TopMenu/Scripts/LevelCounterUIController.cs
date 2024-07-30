using DG.Tweening;
using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class LevelCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelCounterTMP;
        [SerializeField] private float intervalAmount=3f;

        private Sequence _loopSequence;
        public void Initialize()
        {
            levelCounterTMP.transform.localScale = Vector3.one;
            _loopSequence?.Kill(true);
            var data = GameManager.Instance.DataManager.GetData<LevelData>();
            levelCounterTMP.SetText($"Level {data.currentLevelIndex+1}");

            if (GameManager.Instance.LevelManager.CurrentLevel.LevelType is LevelType.Hard or LevelType.SuperHard)
            {
                LoopCounter("Hard");
            }
            else
            {
                _loopSequence?.Kill(true);
            }
        }

        private void LoopCounter(string id)
        {
            var data = GameManager.Instance.DataManager.GetData<LevelData>();
            _loopSequence = DOTween.Sequence();
            _loopSequence.AppendInterval(intervalAmount);
            _loopSequence.Append(levelCounterTMP.transform.DOScale(0f, .5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                levelCounterTMP.SetText(id);
            }));
            _loopSequence.Append(levelCounterTMP.transform.DOScale(1f, .5f).SetEase(Ease.InOutSine));
            _loopSequence.AppendInterval(intervalAmount);
            _loopSequence.Append(levelCounterTMP.transform.DOScale(0f, .5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                levelCounterTMP.SetText($"Level {data.currentLevelIndex + 1}");
            }));
            _loopSequence.Append(levelCounterTMP.transform.DOScale(1f, .5f).SetEase(Ease.InOutSine));
            _loopSequence.SetLoops(-1, LoopType.Restart);
        }
        
    }
}