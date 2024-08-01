using MatchinghamGames.Handyman;
using MatchinghamGames.Localization.Scriptables;
using UnityEngine;
using UnityEngine.Serialization;

namespace MatchinghamGames.DataUsageConsent
{
    public class DataUsageConsentConfig : ModuleConfig
    {
        
#pragma warning disable 649
        [field: SerializeField] public override bool Enabled { get; set; } = true;
        public override bool InternetRequired => false;

        [field: Tooltip("Choose either Google or Unity to determine GDPR compliance")] 
        [field: SerializeField] public GeoLocationDetectionURL GeoLocationDetectionMethod { get; private set; } = GeoLocationDetectionURL.Google;

        [field: Space] [field: Header("Default Popup Settings")]
        
        [field: Tooltip("Assign game objects that have a script that inherits from ConsentPromptViewBase")]
        [field: SerializeField] public GameObject DefaultPopupPrefab { get; private set; }
        
        [field: SerializeField] public int AgeRestriction { get; private set; } = 16;
        [field: SerializeField] public BaseDataStore LocalizationDataStore { get; private set; }

        [field: Space] [field: Header("iOS Specific Settings")]
        
        [field: Tooltip("Use native popup texts instead of old ones")] 
        [field: SerializeField] public bool UseNativeTexts { get; private set; } = true;
        
        [field: Tooltip("Show Native DUCM popup prior to ATT popup")] 
        [field: SerializeField] public bool PreATTPopupEnabled { get; private set; } = true;
        
        [field: TextArea(3, 10)]
        [field: SerializeField] public string UsageDescription { get; private set; } = "Your data will only be used to deliver personalized ads and content to you";
#pragma warning restore 649
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!DefaultPopupPrefab)
            {
                return;
            }

            if (DefaultPopupPrefab.TryGetComponent<ConsentPromptViewBase>(out _)) return;
            
            UnityEditor.EditorUtility.DisplayDialog("Error", "This game object doesn't have a Consent Prompt View script attached!", "My bad");
            DefaultPopupPrefab = null;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif

        public enum GeoLocationDetectionURL
        {
            Google = 0,
            Unity  = 1
        }
    }
}