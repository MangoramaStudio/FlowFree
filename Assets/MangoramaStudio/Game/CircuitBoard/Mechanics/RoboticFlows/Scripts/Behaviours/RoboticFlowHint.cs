using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanics.RoboticFlows
{
    public class RoboticFlowHint : MonoBehaviour
    {
        [SerializeField] private RoboticFlowDrawer drawer;
        [SerializeField] private List<Hint> hints;

        private Hint _highlighting;

        public void SetHints(IEnumerable<Hint> hintList)
        {
            hints = hintList.ToList();
        }

        [Button(ButtonSizes.Large)]
        public void HighlightHint()
        {
            if (_highlighting != null)
                return;
            
            var hintId = drawer.Drawers.Where(d => d.DrawnCells.Count(c => c.node) < 2).Select(d => d.Id).FirstOrDefault();
            var hint = hints.FirstOrDefault(h => h.id == hintId);

            if (hint == null)
                return;

            _highlighting = hint;

            foreach (var cell in hint.cells)
            {
                if (!cell.IsBlinking)
                {
                    cell.SetOccupiedColor(hint.color);
                    cell.Blink(hint.color);
                }
            }
        }

        public void StopHighlight()
        {
            if (_highlighting == null)
                return;
            
            foreach (var cell in _highlighting.cells)
            {
                if (cell.IsBlinking)
                    cell.StopBlink();
            }

            _highlighting = null;
        }

        
        [System.Serializable]
        public class Hint
        {
            public int id;
            [FormerlySerializedAs("hintColor")] public Color color;
            public List<Cell> cells;
        }
    }
}