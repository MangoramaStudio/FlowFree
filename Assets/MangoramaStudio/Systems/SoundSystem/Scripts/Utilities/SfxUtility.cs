using System.Collections.Generic;
using System.Linq;
using MatchinghamGames.ApolloModule;
using Sirenix.OdinInspector;

namespace Utilities
{
    [System.Serializable]
    public static class SfxUtility
    {
        public static IList<ValueDropdownItem<string>> GetSfxKeys()
        {
            #if UNITY_EDITOR
            var filePath = $"{MatchinghamGames.Handyman.MGFolderStructure.ResourcesConfigFolder}/{nameof(ApolloConfig)}.asset";
            var config = UnityEditor.AssetDatabase.LoadAssetAtPath<ApolloConfig>(filePath);

            if (!config)
            {
                return new List<ValueDropdownItem<string>>();
            }
            
            return config.AudioSourcePresets.Select(sfx => new ValueDropdownItem<string>(sfx.Key, sfx.Key)).ToList();
            #else
            return new List<ValueDropdownItem<string>>();
            #endif
        }
    }
}