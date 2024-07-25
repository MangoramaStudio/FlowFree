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


        private int _currentNoteIndex;
        
        public override void Initialize()
        {
            base.Initialize();
            _settingsData = GameManager.Instance.DataManager.GetData<SettingsData>();
        }

        protected override void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.EventManager;
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                eventManager.OnCompleteFlow += PlayCompleteLine;
                eventManager.OnCompleteAllFlows += PlayCompleteLevel;
                eventManager.OnPlayNotes += PlayNotes;
                eventManager.OnIncrementNoteIndex += IncrementNoteIndex;
                eventManager.OnDecrementNoteIndex += DecrementNoteIndex;
                eventManager.OnResetNoteIndex += ResetNoteIndex;

            }
            else
            {
                eventManager.OnCompleteFlow -= PlayCompleteLine;
                eventManager.OnCompleteAllFlows -= PlayCompleteLevel;
                eventManager.OnPlayNotes -= PlayNotes;
                eventManager.OnIncrementNoteIndex -= IncrementNoteIndex;
                eventManager.OnDecrementNoteIndex -= DecrementNoteIndex;
                eventManager.OnResetNoteIndex -= ResetNoteIndex;

            }
        }

        private void PlayNotes()
        {
           var note = musicNotesSfx.ElementAt(_currentNoteIndex % musicNotesSfx.Count);
           Debug.LogWarning(note);
           TryPlaySound(note);
        }


        public IList<ValueDropdownItem<string>> GetSfxKeys()
        {
            return SfxUtility.GetSfxKeys();
        }

        private void IncrementNoteIndex()
        {
            _currentNoteIndex++;
        }

        private void DecrementNoteIndex()
        {
            _currentNoteIndex--;
        }

        private void ResetNoteIndex()
        {
            _currentNoteIndex = 0;
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