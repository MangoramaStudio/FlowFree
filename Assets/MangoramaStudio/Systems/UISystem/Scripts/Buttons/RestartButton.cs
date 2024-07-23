using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class RestartButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            GameManager.Instance.EventManager.RestartLevel();
        }
    }
}