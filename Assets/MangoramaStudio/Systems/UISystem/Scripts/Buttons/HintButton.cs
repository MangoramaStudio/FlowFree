using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class HintButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            
            IsClicked = true;
            GameManager.Instance.EventManager.ShowRewarded(OnRewardedSuccess,OnRewardedFail,"HintButton");
            Debug.Log($"Hint rewarded show sent");

        }
        
        private void OnRewardedSuccess()
        {
            GameManager.Instance.EventManager.RaiseHint();
            IsClicked = false;
            Debug.Log($"Hint rewarded is successful");
        }

        private void OnRewardedFail()
        {
            IsClicked = false;
            Debug.Log($"Hint rewarded is failed");

        }

      
    }
}