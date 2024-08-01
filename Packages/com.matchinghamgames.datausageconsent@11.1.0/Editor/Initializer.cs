using System.Reflection;
using MatchinghamGames.Handyman.Editor;
using MatchinghamGames.Handyman.Editor.Extensions;
using UnityEditor;
using UnityEngine;
using MatchinghamGames.Localization.Scriptables;

namespace MatchinghamGames.DataUsageConsent.Editor
{
    [InitializeOnLoad]
    public static class Initializer
    {
        private static readonly string DefaultPopupPath = $"Packages/{Package.Id}/Runtime/Prefabs/DataUsageConsentPrompt.prefab";
        private static readonly string LocalizationDataStorePath = $"Packages/{Package.Id}/Runtime/Data/DataUsageLocalizationData.asset";
        
        static Initializer()
        {
            MGEditorUtility.FetchConfigAndDo<DataUsageConsentConfig>(Initialize);
        }

        private static void Initialize(DataUsageConsentConfig config)
        {
            if (!config.DefaultPopupPrefab)
            {
                var view = AssetDatabase.LoadAssetAtPath<DetailedConsentView>(DefaultPopupPath);
                if (!view) return;

                var serializedObject = new SerializedObject(config);
                var promptPrefabProperty =
                    serializedObject.FindProperty($"<{nameof(config.DefaultPopupPrefab)}>k__BackingField");
                serializedObject.Update();
                promptPrefabProperty.objectReferenceValue = view.gameObject;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssets();
            }

            if (!config.LocalizationDataStore)
            {
                var localizationDataStore = AssetDatabase.LoadAssetAtPath<BaseDataStore>(LocalizationDataStorePath);
                if (!localizationDataStore) return;

                var serializedObject = new SerializedObject(config);
                var localizedTextsProperty =
                    serializedObject.FindProperty($"<{nameof(config.LocalizationDataStore)}>k__BackingField");
                serializedObject.Update();
                localizedTextsProperty.objectReferenceValue = localizationDataStore;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
