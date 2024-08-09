using System;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
using MatchinghamGames.VegasModule;
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
            if (!Vegas.Interstitial.Enabled)
            {
                RetryLevel();
                return;
            }
                
            if (Vegas.Interstitial.IsCapped)
            {
                RetryLevel();
                return;
            }
                
            GameManager.Instance.EventManager.ShowInterstitial("LevelRetry");

            
        }
        
        private void RetryLevel()
        {
            GameManager.Instance.LevelManager.RetryCurrentLevel();  
            IsClicked = false;
        }

        private void Retry(AdDTO adDto)
        {
            RetryLevel();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                Vegas.Service.Interstitial.Shown += Retry;
            }
            else
            {
                Vegas.Service.Interstitial.Shown -= Retry;
                IsClicked = false;
            }
        }
    }
}