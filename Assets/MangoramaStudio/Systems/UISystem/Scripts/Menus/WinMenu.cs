using MangoramaStudio.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class WinMenu : BaseMenu
    {
        [SerializeField] private Button continueButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            ToggleEvents(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ToggleEvents(false);
        }

        private void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {
                continueButton.onClick.AddListener(Continue);
            }
            else
            {
                continueButton.onClick.RemoveListener(Continue);
            }
        }

        private void Continue()
        {
            GameManager.Instance.LevelManager.ContinueToNextLevel();
        }
    }
}