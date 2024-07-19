using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class FlowDrawerConverter : MonoBehaviour
    {

        private List<FlowDrawer> _flowDrawers = new();
        private List<Cell> _cells = new List<Cell>();
        
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
        
#endif
    }
}