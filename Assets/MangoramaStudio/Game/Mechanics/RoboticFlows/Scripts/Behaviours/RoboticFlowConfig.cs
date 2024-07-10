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
    }
}