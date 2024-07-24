using System;
using System.Threading.Tasks;
using MatchinghamGames.VegasModule;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class ContinueNextLevelButton : BaseButton
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
                GameManager.Instance.EventManager.ShowInterstitial("LevelComplete");
                await Task.Delay(TimeSpan.FromSeconds(.5f), destroyCancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                GameManager.Instance.LevelManager.ContinueToNextLevel();  
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