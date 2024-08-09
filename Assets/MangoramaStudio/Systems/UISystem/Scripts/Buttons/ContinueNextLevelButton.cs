using System;
using System.Threading.Tasks;
using MatchinghamGames.VegasModule;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class ContinueNextLevelButton : BaseButton
    {
        
        protected override void Click()
        {
            base.Click();
            if (IsClicked)
            {
                return;
            }
            
            IsClicked = true;
           

            if (!Vegas.Interstitial.Enabled)
            {
                ContinueNextLevel();
                return;
            }
                
            if (Vegas.Interstitial.IsCapped)
            {
                ContinueNextLevel();
                return;
            }
                
            GameManager.Instance.EventManager.ShowInterstitial("LevelComplete");
            
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                Vegas.Service.Interstitial.Shown += ContinueNextLevel;
            }
            else
            {
                Vegas.Service.Interstitial.Shown -= ContinueNextLevel;
                IsClicked = false;
            }
        }

        private void ContinueNextLevel(AdDTO adTo)
        {
            ContinueNextLevel();
        }

        private void ContinueNextLevel()
        {
            GameManager.Instance.LevelManager.ContinueToNextLevel(); 
            IsClicked = false;
        }
    }
}