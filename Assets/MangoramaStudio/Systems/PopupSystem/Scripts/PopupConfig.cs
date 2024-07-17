using UnityEngine;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    [CreateAssetMenu(fileName = "PopupConfig", menuName = "Config/PopupConfig", order = 0)]
    
    
    public class PopupConfig : ScriptableObject
    {
        public Canvas popupCanvas;
        public SerializableDictionary<PopupType, PopupBase> popups = new();
    }
}