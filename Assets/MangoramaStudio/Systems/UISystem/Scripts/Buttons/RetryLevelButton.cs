using System;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Buttons
{
    public class RetryLevelButton : BaseButton
    {
        protected override async void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            
            IsClicked = true;
            try
            {
                GameManager.Instance.EventManager.ShowInterstitial("LevelRetry");
                await Task.Delay(TimeSpan.FromSeconds(.5f), destroyCancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                GameManager.Instance.LevelManager.RetryCurrentLevel();  
            }
            
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
            }
            else
            {
                IsClicked = false;
            }
        }
    }
}