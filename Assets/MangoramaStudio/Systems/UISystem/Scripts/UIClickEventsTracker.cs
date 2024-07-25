using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers
{
    public class UIClickEventsTracker : MonoBehaviour
    {
        [SerializeField] private string eventName;
        [SerializeField] private bool withParameter;
        private Button Button => _button ? _button : (_button = GetComponent<Button>());
        private Button _button;
        
        
        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void OnDestroy()
        {
            ToggleEvents(false); 
        }

        private void ToggleEvents(bool isToggled)
        {
            if (isToggled)
            {
                Button.onClick.AddListener(Send);  
            }
            else
            {
                Button.onClick.RemoveListener(Send);  
            }
        }

        private void Send()
        {
            GameManager.Instance.EventManager.SendFirebaseEvent(eventName,withParameter);
        }
  
    }
}