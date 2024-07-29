using DG.Tweening;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers
{
    public enum SettingsType
    {
        Sound,
        Haptic
    }
    
    public class ToggleSettingsComponent : BaseButton
    {
        [SerializeField] private SettingsType settingsType;
        [SerializeField] private Image toggle;
        [SerializeField] private RectTransform on;
        [SerializeField] private RectTransform off;
        [SerializeField] private GameObject onColor, offColor;
        
        
        private bool _isToggleOn;
        private bool _isToggled;
        private DataManager DataManager => _dataManager ? _dataManager : (_dataManager = GameManager.Instance.DataManager);
        private DataManager _dataManager;
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            var settingsData = DataManager.GetData<SettingsData>();
            if (settingsType == SettingsType.Haptic)
            {
                _isToggleOn = settingsData.isHapticEnabled == 1;
            }
            else if (settingsType == SettingsType.Sound)
            {
                _isToggleOn = settingsData.isSfxEnabled == 1;
            }
            
            ClickInStart();
        }
        
        private void ClickInStart()
        {
            var sequence = DOTween.Sequence();

            if (_isToggleOn)
            {
                toggle.transform.SetParent(on.transform);
                sequence.Append(toggle.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f).SetEase(Ease.InOutSine));
                onColor.gameObject.SetActive(true);
                offColor.gameObject.SetActive(false);
      
            }
            else
            {
                toggle.transform.SetParent(off.transform);
                sequence.Append(toggle.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f).SetEase(Ease.InOutSine));
                onColor.gameObject.SetActive(false);
                offColor.gameObject.SetActive(true);
              
            }
         
        }


        protected override void Click()
        {
            base.Click();
            if (_isToggled)
            {
                return;
            }
            _isToggled = true;
            var duration = .15f;
            var sequence = DOTween.Sequence();
            if (_isToggleOn)
            {
               
                toggle.transform.SetParent(off.transform);
                sequence.Append(toggle.GetComponent<RectTransform>().DOAnchorPosX(0f, duration).SetEase(Ease.InOutSine));
               onColor.gameObject.SetActive(false);
               offColor.gameObject.SetActive(true);
                _isToggleOn = false;
            }
            else
            {
                toggle.transform.SetParent(on.transform);
                sequence.Append(toggle.GetComponent<RectTransform>().DOAnchorPosX(0f, duration).SetEase(Ease.InOutSine));
               onColor.gameObject.SetActive(true);
               offColor.gameObject.SetActive(false);
                _isToggleOn = true;
            }

            sequence.AppendCallback(() =>
            {
                _isToggled = false;
            });
            
            
            var settingsData = DataManager.GetData<SettingsData>();
            if (settingsType == SettingsType.Haptic)
            {
                settingsData.isHapticEnabled = _isToggleOn ? 1 : 0;
            }
            else if (settingsType == SettingsType.Sound)
            {
                settingsData.isSfxEnabled = _isToggleOn ? 1 : 0;
            }
            
            GameManager.Instance.EventManager.SaveData();
        }
    }
}