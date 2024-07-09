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
            if (PlayerData.IsHapticsEnabled == 1)
            {
                Vibrator.Vibrate(vibrationType);     
            }
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