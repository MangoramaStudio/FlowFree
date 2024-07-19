using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{

    public class FlowDrawerColorToSpriteConverter : MonoBehaviour
    {
        [SerializeField] private RoboticFlowConfig config;
        private FlowDrawer FlowDrawer => _flowDrawer ? _flowDrawer : (_flowDrawer = GetComponent<FlowDrawer>());
        private FlowDrawer _flowDrawer;

#if UNITY_EDITOR

        [Button]
        public void Convert()
        {
            var findSprite = config.flowTileDefinitions.Find(x => IsEqualTo(x.color,FlowDrawer.GetColor()));
            var tile =findSprite.tile;
            FlowDrawer.SetTileSprite(tile);   
        }
        
        public  bool IsEqualTo(Color me, Color other)
        {
            return Math.Abs(me.r - other.r) < .1f && Math.Abs(me.g - other.g) < .1f && Math.Abs(me.b - other.b) < .1f && Math.Abs(me.a - other.a) < .1f;
        }

#endif
    }
}