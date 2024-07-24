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
            GameManager.Instance.EventManager.ShowInterstitial("LevelComplete");
            await Task.Yield();
            GameManager.Instance.LevelManager.ContinueToNextLevel();
            
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