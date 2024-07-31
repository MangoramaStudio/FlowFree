using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                eventManager.OnPlayNotes += PlayNotes;
                eventManager.OnIncrementNoteIndex += IncrementNoteIndex;
                eventManager.OnDecrementNoteIndex += DecrementNoteIndex;
                eventManager.OnResetNoteIndex += ResetNoteIndex;
                eventManager.OnPlayLevelSuccess += PlayCompleteLevel;
                eventManager.OnPlayFlowSuccess += PlayCompleteLine;
                eventManager.OnRestartLevel += ResetData;
            }
            else
            {
                eventManager.OnPlayNotes -= PlayNotes;
                eventManager.OnIncrementNoteIndex -= IncrementNoteIndex;
                eventManager.OnDecrementNoteIndex -= DecrementNoteIndex;
                eventManager.OnResetNoteIndex -= ResetNoteIndex;
                eventManager.OnPlayLevelSuccess -= PlayCompleteLevel;
                eventManager.OnPlayFlowSuccess -= PlayCompleteLine;
                eventManager.OnRestartLevel -= ResetData;
            }
        }

        private void ResetData()
        {
            ResetNoteIndex();
        }

        private void PlayNotes()
        {
            // TODO Irfan solve bug here none element
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

        private bool _addToDecrement;

        private void DecrementNoteIndex(int index)
        {
            if (!_addToDecrement)
            {
                _currentNoteIndex = index;
                _addToDecrement = true;
            }
       
            _currentNoteIndex--;
            
            if (_currentNoteIndex<=0)
            {
                _currentNoteIndex = 0;
            }
        
        }

        private void ResetNoteIndex()
        {
            _addToDecrement = false;
            _currentNoteIndex = 0;
        }
        
        private void PlayCompleteLine()
        {
            TryPlaySound(singleMatchSfx);
        }

        private async void PlayCompleteLevel()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(.5f), destroyCancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                TryPlaySound(fullMatchSfx);     
            }
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