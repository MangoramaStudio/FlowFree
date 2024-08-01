#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MatchinghamGames.Handyman.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace MatchinghamGames.DataUsageConsent.Editor
{
    internal static class BuildPostProcessor
    {
        [PostProcessBuild(1)]
        private static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS) return;
            
            MGEditorUtility.FetchConfigAndDo<DataUsageConsentConfig>(config =>
            {
                // Get plist
                string plistPath = pathToBuiltProject + "/Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
       
                // Get root
                PlistElementDict rootDict = plist.root;
       
                // Set value of NSUserTrackingUsageDescription in Xcode plist
                var nsUserTrackingUsageDescription = "NSUserTrackingUsageDescription";
                rootDict.SetString(nsUserTrackingUsageDescription, config.UsageDescription);
       
                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            });
        }
    }
}
#endif
