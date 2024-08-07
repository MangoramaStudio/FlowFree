using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Scripts.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class FlowDrawerConverter : MonoBehaviour
    {

        public bool solvedManually;
        private List<FlowDrawer> _flowDrawers = new();
        private List<Cell> _cells = new List<Cell>();
        private RoboticFlowHint _hint;
        
#if UNITY_EDITOR

        public void ConvertToCells(bool isStroke6)
        {
            _cells = GetComponentsInChildren<Cell>().ToList();
            
            foreach (var cell in _cells)
            {
                cell.GetComponent<CellGridColorToSpriteConverter>().ChangeCells(isStroke6);
            }
        }
        
        
        [Button]
        public void Convert()
        {
            
            _flowDrawers = GetComponentsInChildren<FlowDrawer>().ToList();
            _cells = GetComponentsInChildren<Cell>().ToList();
            foreach (var drawer in _flowDrawers)
            {
                drawer.GetComponent<FlowDrawerColorToSpriteConverter>().Convert();
                drawer.transform.localPosition = new Vector3(0, 0f, 0);
            }

            foreach (var cell in _cells)
            {
                cell.GetComponent<CellGridColorToSpriteConverter>().Convert();
            }
        }

        [Button]
        public void AddCorrectOrderCells(Vector2Int size,LevelBehaviour levelBehaviour)
        {
            if (solvedManually)
            {
                return;
            }
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