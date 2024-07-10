using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class HintButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            GameManager.Instance.EventManager.RaiseHint();
        }
    }
}