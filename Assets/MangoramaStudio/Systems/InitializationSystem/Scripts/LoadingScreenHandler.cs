using DG.Tweening;
using MatchinghamGames.GameUtilities.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.SDK
{
    public class LoadingScreenHandler : MonoBehaviour
    {
        [SerializeField] private Slider loadingBar;
        private Tween _sliderTween;
        private void Start()
        { 
            ToggleEvents(true);
        }
        private void OnDestroy()
        { 
            ToggleEvents(false);
        }
        private void IncrementProgress(float value)
        {
            if (value>=1f)
            {
                ToggleEvents(false);
                return;
            }
            
            _sliderTween?.Kill();
            _sliderTween = loadingBar.DOValue(value, .2f).SetEase(Ease.Linear);
        }

        private void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {
                GameInitializeManager.Instance.InitializationProgressChanged += IncrementProgress;   
            }
            else
            {
                GameInitializeManager.Instance.InitializationProgressChanged -= IncrementProgress;   
            }
        }
    }
}