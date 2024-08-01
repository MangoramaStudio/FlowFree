using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

#if TMP || UGUI_2_OR_NEWER
using MatchinghamGames.Handyman;
using MatchinghamGames.PrivacyPolicyPopup;
using TMPro;
#endif

namespace MatchinghamGames.DataUsageConsent
{
    public class DetailedConsentView : ConsentPromptViewBase, IPointerClickHandler
    {
        
        private static readonly int close = Animator.StringToHash("close");
        
        #pragma warning disable 649
        [SerializeField] private Animator popupAnimator;
        
        [Header("Consent Page")]
#if TMP || UGUI_2_OR_NEWER
        [SerializeField] private TMP_Text thanksForDownloadingText;
        [SerializeField] private TMP_Text consentText;
#endif
        #pragma warning restore 649
        
        [Header("Button")]
        [SerializeField] private Button okButton;

       
        protected virtual void OnValidate()
        {
#if TMP || UGUI_2_OR_NEWER
            if (thanksForDownloadingText == null)
            {
                Debug.LogWarning($"{nameof(thanksForDownloadingText)} is null, is there a missing reference?");
            }
            
            if (consentText == null)
            {
                Debug.LogException(new NullReferenceException($"{nameof(consentText)} cannot be null."));
            }
#endif
            
            if (okButton == null)
            {
                Debug.LogException(new NullReferenceException($"{nameof(okButton)}; 'Confirm Button' cannot be null."));
            }
            
            if (popupAnimator == null)
            {
                Debug.LogException(new MissingReferenceException($"{nameof(popupAnimator)} component is missing."));
            }
        }

        private void OnDisable()
        {
            DisposeConsentPage();
        }

        public override void Initialize()
        {
#if TMP || UGUI_2_OR_NEWER
            thanksForDownloadingText.text = string.Format(thanksForDownloadingText.text, Application.productName);
#endif

            okButton.onClick.AddListener(OnOkClick);
        }

        private void DisposeConsentPage()
        {
            okButton.onClick.RemoveListener(OnOkClick);
        }

        private void OnOkClick()
        {
            DataUsageConsentManager.SetUserConsent(UserResponse.Yes);
            
            popupAnimator.SetBool(close, true);
        }

        [UsedImplicitly]
        private void OnClose()
        {
            NotifyPromptDismissed();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
#if TMP || UGUI_2_OR_NEWER
            var linkInfo = new TMP_LinkInfo();

            var index = TMP_TextUtilities.FindIntersectingLink(consentText, eventData.position,
                eventData.pressEventCamera);
            if (index != -1)
            {
                linkInfo = consentText.textInfo.linkInfo[index];
            }
            
            var selectedLink = linkInfo.GetLinkID();
            
            switch (selectedLink)
            {
                case "privacy":
                    OpenPrivacyPolicy();
                    break;
                
                case "terms":
                    OpenTermsAndConditions();
                    break;
            }
#endif
        }
        
        private static void OpenPrivacyPolicy()
        {
            PrivacyPolicy.DisplayPrivacyPolicy();
        }

        private static void OpenTermsAndConditions()
        {
            PrivacyPolicy.DisplayTermsAndConditions();
        }

    }
}