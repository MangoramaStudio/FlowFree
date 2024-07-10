using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class HintButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            // TODO Add rewarded here
            GameManager.Instance.EventManager.RaiseHint();
        }
    }
}