using UnityEngine;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class WarningOkButton : BaseButton
    {
        [SerializeField] private GameObject warningPanel;
        protected override void Click()
        {
            base.Click();
            Destroy(warningPanel.gameObject);
        }
    }
}