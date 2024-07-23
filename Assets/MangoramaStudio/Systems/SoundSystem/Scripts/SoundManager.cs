using System.Collections.Generic;
using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.ApolloModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace MangoramaStudio.Systems.SoundSystem.Scripts
{
    
    public class SoundManager : BaseManager
    {
        
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string singleMatchSfx;
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string fullMatchSfx;
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string drawSfx;
        
        
        public IList<ValueDropdownItem<string>> GetSfxKeys()
        {
            return SfxUtility.GetSfxKeys();
        }
        
        public void PlayDrawLine()
        {
            TryPlaySound(drawSfx);
        }

        public void PlayCompleteLine()
        {
            TryPlaySound(singleMatchSfx);
        }

        public void PlayCompleteLevel()
        {
            TryPlaySound(fullMatchSfx);
        }
        
        private void TryPlaySound(string id)
        {
            if (PlayerData.IsSfxEnabled == 1)
            {
                Apollo.PlaySingleAudio(id);          
            }
        }
    }
}