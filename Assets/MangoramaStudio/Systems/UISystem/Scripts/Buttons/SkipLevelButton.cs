using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class SkipLevelButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            IsClicked = true;
            GameManager.Instance.EventManager.ShowRewarded(OnRewardedSuccess,OnRewardedFail,"SkipLevel");
            Debug.Log($"Skip Level rewarded show sent");
        }

        private void OnRewardedSuccess()
        {
            GameManager.Instance.EventManager.AutoComplete();
            IsClicked = false;
        }
        private void OnRewardedFail()
        {
            IsClicked = false;
        }

      
    }
}