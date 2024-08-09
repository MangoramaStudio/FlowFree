using DG.Tweening;
using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.RoboticFlows
{
    public class LevelCounterUIController : UIBehaviour
    {
        [SerializeField] private Image headerBg;
        [SerializeField] private TextMeshProUGUI levelCounterTMP,definitionTMP,sizeTMP;
        [SerializeField] private GameObject ballDefinitionObject,ballImageObject;
        [SerializeField] private float intervalAmount=3f;

        private Sequence _loopSequence;
        public void Initialize()
        {
            _loopSequence?.Kill(true);
            DOTween.Kill(_loopSequence);
            definitionTMP.transform.localScale = Vector3.zero;
            sizeTMP.transform.localScale = Vector3.one;
            ballImageObject.transform.localScale = Vector3.one;
            levelCounterTMP.transform.localScale = Vector3.one;
            
            var getSize = GameManager.Instance.LevelManager.CurrentLevel.Container.Builder.GetSize();
            var sizeText = $"{getSize.x}x{getSize.y}";
            sizeTMP.SetText(sizeText);
            var data = GameManager.Instance.DataManager.GetData<LevelData>();
            levelCounterTMP.SetText($"Level {data.currentLevelIndex}");

            if (GameManager.Instance.LevelManager.CurrentLevel.LevelType is LevelType.Hard)
            {
                LoopCounter("Hard");
            }
            else if (GameManager.Instance.LevelManager.CurrentLevel.LevelType is LevelType.SuperHard)
            {
                LoopCounter("Super Hard");
            }
            else
            {
                _loopSequence?.Kill(true);
                sizeTMP.transform.localScale = Vector3.one;
                ballImageObject.transform.localScale = Vector3.one;
            }
            
            SetTheme(GameManager.Instance.LevelManager.CurrentLevel.LevelType);
        }
        
        private void SetTheme(LevelType levelType)
        {
            var definition = GameManager.Instance.UIManager.GameplayMenu().LevelTypeMenuDefinitions.Find(x => x.levelType == levelType);
            headerBg.sprite = definition.topMenuLevelHeaderBg;
            definitionTMP.color = definition.counterColor;
            sizeTMP.color = definition.counterColor;
         
        }

        private void LoopCounter(string id)
        {
            var data = GameManager.Instance.DataManager.GetData<LevelData>();
            _loopSequence = DOTween.Sequence();
            _loopSequence.AppendInterval(intervalAmount);
            _loopSequence.Append(ballDefinitionObject.transform.DOScale(0f, .5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                definitionTMP.transform.localScale = Vector3.one;
                sizeTMP.transform.localScale = Vector3.zero;
                ballImageObject.transform.localScale = Vector3.zero;
                definitionTMP.SetText(id);
            }));
            _loopSequence.Append(ballDefinitionObject.transform.DOScale(1f, .5f).SetEase(Ease.InOutSine));
            _loopSequence.AppendInterval(intervalAmount);
            _loopSequence.Append(ballDefinitionObject.transform.DOScale(0f, .5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                definitionTMP.transform.localScale = Vector3.zero;
                sizeTMP.transform.localScale = Vector3.one;
                ballImageObject.transform.localScale = Vector3.one;
            }));
            _loopSequence.Append(ballDefinitionObject.transform.DOScale(1f, .5f).SetEase(Ease.InOutSine));
            _loopSequence.SetLoops(-1, LoopType.Restart);
        }
        
    }
}