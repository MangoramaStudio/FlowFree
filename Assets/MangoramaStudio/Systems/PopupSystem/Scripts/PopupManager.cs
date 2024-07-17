using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    public class PopupManager : BaseManager
    {
        [SerializeField] private PopupConfig popupConfig;
        public PopupConfig PopupConfig => popupConfig;

        private readonly Dictionary<PopupType, PopupBase> _currentPopups = new();

        private Canvas _popupCanvas;

        public override void Initialize()
        {
            base.Initialize();
            _popupCanvas = Instantiate(PopupConfig.popupCanvas);
        }

        [Button]
        public void Show(PopupType popupType)
        {
            if (_currentPopups.ContainsKey(popupType))
            {
                Debug.LogError("There is already a popup in scene");
                return;
            }
            var popup = Cast(popupType);
            var p = Instantiate(popup, _popupCanvas.transform);
            _currentPopups.TryAdd(popupType,p);
            p.Show();
        }

        [Button]
        public void Hide(PopupType popupType)
        {
            var hidePopup =_currentPopups.FirstOrDefault(x => x.Key == popupType).Value;
            if (hidePopup == null) return;
            _currentPopups.Remove(popupType);
            hidePopup.Hide();
        }
        private PopupBase Cast(PopupType popupType)
        {
            var popup = PopupConfig.popups.FirstOrDefault(x => x.Key == popupType).Value;
            return popup ==null ? null : popup;
        }
    }
    
}