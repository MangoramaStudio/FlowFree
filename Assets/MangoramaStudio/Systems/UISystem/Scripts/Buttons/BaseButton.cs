using MangoramaStudio.Scripts.Data;
using MatchinghamGames.VibrationModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class BaseButton : UIBehaviour
    {

        [SerializeField] private VibrationType vibrationType = VibrationType.Medium;
        protected Button Button => _button ? _button : (_button = GetComponent<Button>());
        private Button _button;
        
        protected bool IsClicked;
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ToggleEvents(false);
        }

        protected virtual void Click()
        {
            TryVibrate();
        }

        private void TryVibrate()
        {
            GameManager.Instance.VibrationManager.VibrateButton(vibrationType);
        }

        public void EnableButton()
        {
            Button.enabled = true;
        }

        public void DisableButton()
        {
            Button.enabled = false;
        }

        protected virtual void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {
                Button.onClick.AddListener(Click);   
            }
            else
            {
                Button.onClick.RemoveListener(Click);   
            }
        }
    }
}