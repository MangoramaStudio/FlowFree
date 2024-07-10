using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class ContinueNextLevelButton : BaseButton
    {

        private bool _isClicked;
        protected override void Click()
        {
            base.Click();
            if (_isClicked)
            {
                return;
            }
            _isClicked = true;
            GameManager.Instance.LevelManager.ContinueToNextLevel();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (!isToggled)
            {
                _isClicked = false;
            }
        }
    }
}