using System;
using UnityEngine;

namespace MatchinghamGames.DataUsageConsent
{
    public abstract class ConsentPromptViewBase : MonoBehaviour
    {
        public Action OnPromptDismissed;
        
        public abstract void Initialize();
        
        protected void NotifyPromptDismissed()
        {
            OnPromptDismissed?.Invoke();
        }
    }
}