using System.Collections.Generic;
using System.Linq;
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
        private SettingsData _settingsData;

        
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string singleMatchSfx;
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string fullMatchSfx;
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string drawSfx;
        [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private List<string> musicNotesSfx = new();

        
        
        public override void Initialize()
        {
            base.Initialize();
            _settingsData = GameManager.Instance.DataManager.GetData<SettingsData>();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
               // GameManager.Instance.EventManager.OnDrawCell += PlayDrawLine;
                GameManager.Instance.EventManager.OnCompleteFlow += PlayCompleteLine;
                GameManager.Instance.EventManager.OnCompleteAllFlows += PlayCompleteLevel;
                GameManager.Instance.EventManager.OnPlayNotes += PlayNotes;

            }
            else
            {
               // GameManager.Instance.EventManager.OnDrawCell -= PlayDrawLine;
                GameManager.Instance.EventManager.OnCompleteFlow -= PlayCompleteLine;
                GameManager.Instance.EventManager.OnCompleteAllFlows -= PlayCompleteLevel;
                GameManager.Instance.EventManager.OnPlayNotes -= PlayNotes;

            }
        }

        private void PlayNotes(int index)
        {
           var note = musicNotesSfx.ElementAt(index);
           Debug.LogWarning(note);
           TryPlaySound(note);
        }


        public IList<ValueDropdownItem<string>> GetSfxKeys()
        {
            return SfxUtility.GetSfxKeys();
        }

        private void PlayDrawLine()
        {
            TryPlaySound(drawSfx);
        }

        private void PlayCompleteLine()
        {
            TryPlaySound(singleMatchSfx);
        }

        private void PlayCompleteLevel()
        {
            TryPlaySound(fullMatchSfx);
        }
        
        private void TryPlaySound(string id)
        {
            if (_settingsData.isSfxEnabled == 1)
            {
                Apollo.PlaySingleAudio(id);          
            }
        }
    }
}