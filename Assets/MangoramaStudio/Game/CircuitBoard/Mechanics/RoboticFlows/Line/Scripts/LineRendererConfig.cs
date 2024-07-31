using System;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Game.CircuitBoard.Mechanics.RoboticFlows.Line.Scripts
{
    [CreateAssetMenu(fileName = "LineRendererConfig", menuName = "Config/LineRendererConfig", order = 0)]
    public class LineRendererConfig : ScriptableObject
    {
        [SerializeField] private Material material;
        [SerializeField] private List<LineDefinition> lineDefinitions = new();

        public Material Material => material;
        public List<LineDefinition> LineDefinitions => lineDefinitions;
    }

    [Serializable]
    public struct LineDefinition
    {
        public int id;
        public Texture texture;
    }
}