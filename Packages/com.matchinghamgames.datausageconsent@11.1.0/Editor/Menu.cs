using MatchinghamGames.Handyman.Editor;
using UnityEditor;

namespace MatchinghamGames.DataUsageConsent.Editor
{
    public static class Menu
    {
        public const string MenuPath = "Matchingham/Data Usage Consent/";
        
        [MenuItem(MenuPath + "Config")]
        private static void SelectDataUsageConsentConfig()
        {
            MGEditorUtility.FetchConfigAndDo<DataUsageConsentConfig>(config => Selection.activeObject = config);
        }
    }
}