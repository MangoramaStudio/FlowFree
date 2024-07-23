using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class UndoButton : BaseButton
    {
        protected override void Click()
        {
            base.Click();
            GameManager.Instance.EventManager.Undo();
        }
    }
}