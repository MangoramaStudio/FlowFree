using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.PrivacyPolicyPopup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    public class SettingsPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI privacyPolicyTMP, termsOfUseTMP;


        protected override void Start()
        {
            base.Start();
            privacyPolicyTMP.GetComponent<Button>().onClick.AddListener(ClickPrivacyPolicy);
            termsOfUseTMP.GetComponent<Button>().onClick.AddListener(ClickTermsOfUse);

        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            privacyPolicyTMP.GetComponent<Button>().onClick.RemoveListener(ClickPrivacyPolicy);
            termsOfUseTMP.GetComponent<Button>().onClick.RemoveListener(ClickTermsOfUse);

        }


        private void ClickPrivacyPolicy()
        {
            GameManager.Instance.VibrationManager.VibrateButton();
            Application.OpenURL(PrivacyPolicy.PrivacyPolicyURL);
        }
        
        
        private void ClickTermsOfUse()
        {
            GameManager.Instance.VibrationManager.VibrateButton();
            Application.OpenURL(PrivacyPolicy.TermsAndConditionsURL);

        }

    }
}