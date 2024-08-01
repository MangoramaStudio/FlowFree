#if UNITY_IOS
using System;
using System.Collections.Generic;
using MatchinghamGames.Localization;
using MatchinghamGames.Localization.Helpers;
using MatchinghamGames.Localization.Scriptables;
using MatchinghamGames.Utilities;
using TMPro;
using UnityEngine;
using Utilities;


namespace MatchinghamGames.DataUsageConsent
{
    public class DataConsentLocalizedTexts : LocalizedViewBase
    {
        
        private struct Keys
        {
            public const string NativeConsentCheck = nameof(NativeConsentCheck);
            public const string NativeConsentParagraph = nameof(NativeConsentParagraph);
            public const string OK = nameof(OK);
            public const string DataConsentCheckPP = nameof(DataConsentCheckPP);
            public const string DataConsentCheckToS = nameof(DataConsentCheckToS);
        }
        
        public BaseDataStore dataStore;
        public string p1Text = "To use \"{0}\" please agree to our {1} and {2}. User privacy is important to us.";
        public string ppText = "Privacy Policy";
        public string tcText = "Terms And Conditions";
        public string pressOkText = "Please press \"OK\" to agree and start using our app.";
        public string okText = "OK";
        protected override void Initialize()
        {
            dataStore = DataUsageConsentManager.Config.LocalizationDataStore;
            localizer = new BasicLocalizeHelper(dataStore);
            localizer.InitializeDefault();
        }

        public override void OnFontChange(TMP_FontAsset newFont)
        {
            
        }

        public override void OnLanguageChange()
        {
            p1Text = localizer.Get(Keys.NativeConsentParagraph,p1Text);
            pressOkText = localizer.Get(Keys.NativeConsentCheck,pressOkText);
            ppText = localizer.Get(Keys.DataConsentCheckPP,ppText);
            tcText = localizer.Get(Keys.DataConsentCheckToS,tcText);
            okText = localizer.Get(Keys.OK,okText);
        }

        public void OnPreAttCallback()
        {
            DataUsageConsentManager.InvokePreAttPopupClosed();
        }
    }
}
#endif