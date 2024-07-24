using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Scripts.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class FlowDrawerConverter : MonoBehaviour
    {

        private List<FlowDrawer> _flowDrawers = new();
        private List<Cell> _cells = new List<Cell>();
        private RoboticFlowHint _hint;
        
#if UNITY_EDITOR

        [Button]
        public void Convert()
        {
            
            _flowDrawers = GetComponentsInChildren<FlowDrawer>().ToList();
            _cells = GetComponentsInChildren<Cell>().ToList();
            foreach (var drawer in _flowDrawers)
            {
                drawer.GetComponent<FlowDrawerColorToSpriteConverter>().Convert();
                drawer.transform.localPosition = new Vector3(0, -1.5f, 0);
            }

            foreach (var cell in _cells)
            {
                cell.GetComponent<CellGridColorToSpriteConverter>().Convert();
            }
        }

        [Button]
        public void AddCorrectOrderCells(Vector2Int size,LevelBehaviour levelBehaviour)
        {
            _hint = GetComponentInChildren<RoboticFlowHint>();
            _flowDrawers = GetComponentsInChildren<FlowDrawer>().ToList();

            for (int i = 0; i < _flowDrawers.Count; i++)
            {
                var drawer = _flowDrawers[i];
                drawer.AddCorrectOrderCells(_hint,size,levelBehaviour);
            }
        }
        
#endif
    }
}