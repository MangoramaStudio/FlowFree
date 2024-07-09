using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers.Buttons
{
    public class BaseButton : UIBehaviour
    {
        
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
        
        public virtual void Click() { }

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