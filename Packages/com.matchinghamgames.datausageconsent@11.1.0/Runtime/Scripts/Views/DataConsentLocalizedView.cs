using System;
using System.Collections.Generic;
using MatchinghamGames.Localization;
using MatchinghamGames.Localization.Helpers;
using MatchinghamGames.Localization.Scriptables;
using MatchinghamGames.Utilities;
using UnityEngine;
using Utilities;

#if TMP || UGUI_2_OR_NEWER
using TMPro;
#endif


namespace MatchinghamGames.DataUsageConsent
{
    public class DataConsentLocalizedView : LocalizedViewBase
    {
        private struct Keys
        {
            public const string ThanksForDownloading = nameof(ThanksForDownloading);
            public const string NativeConsentParagraph = nameof(NativeConsentParagraph);
            public const string DataConsentParagraph = nameof(DataConsentParagraph);
            public const string DataConsentCheck = nameof(DataConsentCheck);
            public const string DataConsentCheckPP = nameof(DataConsentCheckPP);
            public const string DataConsentCheckToS = nameof(DataConsentCheckToS);
            public const string ConfirmAndPlay = nameof(ConfirmAndPlay);
            public const string OK = nameof(OK);
            // public const string PrivacyPolicy = nameof(PrivacyPolicy);
            // public const string TermsOfService = nameof(TermsOfService);
        }

        [SerializeField] private BaseDataStore dataStore;

#if TMP || UGUI_2_OR_NEWER
        [SerializeField] private TMP_Text ThanksForDownloadingText;
        [SerializeField] private TMP_Text DataConsentCheckText;
        [SerializeField] private TMP_Text ConfirmAndPlayText;
#endif

        private string dataConsentCheckMainText = "";
        private string dataConsentParagraph = "";
        private string dataConsentParagraphNew = "";
        private string dataConsentCheckPpPart = "Privacy Policy";
        private string dataConsentCheckTosPart = "Terms of Service";

        const string highlightFormat = "<link=\"{0}\"><color=#E43B68><b>{1}</b></color></link>";

        protected virtual void OnValidate()
        {
            if (dataStore == null)
            {
                DataUsageConsentManager.Instance.Logger.Fatal($"{nameof(dataStore)} is needed for localization, cannot be null.");
            }

#if TMP || UGUI_2_OR_NEWER
            if (ThanksForDownloadingText == null)
            {
                DataUsageConsentManager.Instance.Logger.Warn($"{nameof(ThanksForDownloadingText)} is null.");
            }
            
            if (ConfirmAndPlayText == null)
            {
                DataUsageConsentManager.Instance.Logger.Warn($"{nameof(ConfirmAndPlayText)} is null.");
            }

            if (DataConsentCheckText == null)
            {
                DataUsageConsentManager.Instance.Logger.Fatal($"{nameof(DataConsentCheckText)} is needed for localization, cannot be null.");
            }
#endif
        }

        protected override void Initialize()
        {
            localizer = new BasicLocalizeHelper(dataStore);
            localizer.InitializeDefault();
        }

        public override void OnLanguageChange()
        {
#if TMP || UGUI_2_OR_NEWER
            if (ThanksForDownloadingText != null)
            {
                ThanksForDownloadingText.text = localizer.Get(Keys.ThanksForDownloading);    
            }

            if (ConfirmAndPlayText != null)
            {
                ConfirmAndPlayText.text = localizer.Get(DataUsageConsentManager.Config.UseNativeTexts ? Keys.OK : Keys.ConfirmAndPlay);
            }
#endif
            
            dataConsentCheckMainText = localizer.Get(Keys.DataConsentCheck);
            dataConsentCheckPpPart = localizer.Get(Keys.DataConsentCheckPP);
            dataConsentCheckTosPart = localizer.Get(Keys.DataConsentCheckToS);
            dataConsentParagraph = localizer.Get(Keys.DataConsentParagraph);
            dataConsentParagraphNew = localizer.Get(Keys.NativeConsentParagraph);

            SetConsentCheckText();
        }

#if TMP || UGUI_2_OR_NEWER
        public override void OnFontChange(TMP_FontAsset newFont)
        {
            if (ThanksForDownloadingText != null)
            {
                ThanksForDownloadingText.font = newFont;    
            }

            if (ConfirmAndPlayText != null)
            {
                ConfirmAndPlayText.font = newFont;    
            }
            
            DataConsentCheckText.font = newFont;
        }
#endif

        private void SetConsentCheckText()
        {
            if (string.IsNullOrEmpty(dataConsentCheckMainText) ||
                string.IsNullOrEmpty(dataConsentCheckPpPart) ||
                string.IsNullOrEmpty(dataConsentCheckTosPart) ||
                string.IsNullOrEmpty(dataConsentParagraph))
            {
                return;
            }

            string ppPartHighlighted = string.Format(highlightFormat, "privacy", dataConsentCheckPpPart);
            string tosPartHighlighted = string.Format(highlightFormat, "terms", dataConsentCheckTosPart);

            try
            {
                var ageRestriction = DataUsageConsentManager.Config.AgeRestriction;
                var companyName = Application.companyName;

              //  dataConsentParagraphNew = string.Format(dataConsentParagraphNew, Application.productName, ppPartHighlighted,
                   // tosPartHighlighted);

#if TMP || UGUI_2_OR_NEWER
                if (DataUsageConsentManager.Config.UseNativeTexts)
                {
                   // DataConsentCheckText.text = dataConsentParagraphNew;
                    ConfirmAndPlayText.text = localizer.Get(Keys.OK);
                }
                else
                {
                  //  DataConsentCheckText.text = string.Format($"{dataConsentParagraph}\n\n{dataConsentCheckMainText}",
                    //    ppPartHighlighted, tosPartHighlighted, companyName, ageRestriction);
                    ConfirmAndPlayText.text = localizer.Get(Keys.ConfirmAndPlay);
                }
                
#endif
            }
            catch (Exception e)
            {
                DataUsageConsentManager.Instance.Logger.Fatal($"Format error in: {dataConsentCheckMainText}", e);
            }
        }
    }
}