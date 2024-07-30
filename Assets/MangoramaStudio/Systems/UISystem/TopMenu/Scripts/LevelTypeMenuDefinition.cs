using System;
using MangoramaStudio.Scripts.Behaviours;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    [Serializable]
    public struct LevelTypeMenuDefinition
    {
        public LevelType levelType;
        public Sprite topMenuPipeBg;
        public Sprite topMenuPipeHeaderBg;
        public Color counterColor;
        public Sprite topHeader;
        public Sprite bottomHeader;
        public Material headerTMPMaterial;
    }
}