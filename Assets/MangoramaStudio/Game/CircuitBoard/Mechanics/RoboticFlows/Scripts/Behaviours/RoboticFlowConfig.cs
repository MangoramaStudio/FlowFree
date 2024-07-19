using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    [CreateAssetMenu(fileName = "RoboticFlowConfig", menuName = "Game/RoboticFlowConfig", order = 0)] 
    public class RoboticFlowConfig : ScriptableObject
    {
        public Color cellColor;
        public Color occupiedCellColor;
        public List<Color> colors;

        public List<FlowTileDefinition> flowTileDefinitions = new();
        public FlowTileDefaultDefinition defaultDefinition;
    }

    [Serializable]
    public struct FlowTileDefinition
    {
        public Color color;
        public Sprite tile;
        public Sprite ball;
    }

    [Serializable]
    public struct FlowTileDefaultDefinition
    {
        public Color color;
        public Sprite tile;

    }
}