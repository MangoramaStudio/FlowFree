using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class CellGridColorToSpriteConverter : MonoBehaviour
    {
        [SerializeField] private RoboticFlowConfig config;
        private Cell Cell => _cell ? _cell : (_cell = GetComponent<Cell>());
        private Cell _cell;
        
        
#if UNITY_EDITOR

        [Button]
        public void Convert()
        {
            var node = Cell.GetComponentInChildren<Node>();
            if (node == null)
            {
                Cell.SetTileSprite(config.defaultDefinition.tile);
                
            }
            else if (node!=null)
            {
                var definition = config.flowTileDefinitions.Find(x => IsEqualTo(x.color,node.Color));
                node.SetBallSprite(definition.ball);
                Cell.SetTileSprite(definition.tile);
            }

            Cell.SetSpriteRenderer();
           
            
        }
        
        public  bool IsEqualTo(Color me, Color other)
        {
            return Math.Abs(me.r - other.r) < .1f && Math.Abs(me.g - other.g) < .1f && Math.Abs(me.b - other.b) < .1f && Math.Abs(me.a - other.a) < .1f;
        }

#endif
    }
}