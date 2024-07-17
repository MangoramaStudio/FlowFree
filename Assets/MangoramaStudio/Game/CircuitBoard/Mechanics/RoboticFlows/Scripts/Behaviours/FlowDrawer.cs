using System.Collections.Generic;
using System.Linq;
using MatchinghamGames.VibrationModule;
using Shapes;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class FlowDrawer : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private Color color;
        [SerializeField] private float occupiedAlpha;
        [SerializeField] private Polyline polyline;
        [SerializeField] private SpriteRenderer tip;

        private Stack<Cell> _drawnCells;

        public int Id => id;

        public bool FlowComplete => _drawnCells.Count(c => c.node) == 2;

        public Stack<Cell> DrawnCells => _drawnCells;

        public Cell CurrentCell => DrawnCells?.Peek();

        public Polyline Polyline => polyline;

        public void Initialize()
        {
            _drawnCells = new Stack<Cell>();
            polyline.enabled = false;
            tip.enabled = false;
        }

        public void Prepare()
        {
            polyline.Color = color;
            tip.color = color;
        }

        public void Clear()
        {
            foreach (var cell in _drawnCells)
            {
                cell.SetOccupied(false);
            }
            
            _drawnCells.Clear();
            polyline.points.Clear();
            polyline.enabled = false;
            tip.enabled = false;
        }

        public void SetColor(Color c)
        {
            color = c;
            polyline.Color = color;
            tip.color = color;
        }

        public void SetId(int i)
        {
            id = i;
        }

        public void DrawCell(Cell cell)
        {
            if (_drawnCells.Contains(cell))
                return;

            if (cell.node && cell.node.Id != id)
                return;
            
            if (FlowComplete)
                return;
            
            _drawnCells.Push(cell);
            
            cell.SetOccupied(true);
            cell.SetOccupiedColor(GetOccupiedColor());
            cell.ShowFillHint(false);

            var position = cell.transform.position - transform.position;
            polyline.AddPoint(new Vector3(position.x, position.z));
            tip.transform.position = cell.transform.position;

            if (cell.node)
            {
                Vibrator.Vibrate(VibrationType.Rigid);
            }
            else
            {
                Vibrator.Vibrate(VibrationType.Light);
            }

            if (polyline.Count > 1)
            {
                tip.enabled = true;
                polyline.enabled = true;
            }
        }

        public void ClearToCell(Cell cell)
        {
            while (_drawnCells.Count > 0 && _drawnCells.Peek() != cell)
            {
                var popped = _drawnCells.Pop();
                popped.SetOccupied(false);
                polyline.points.RemoveAt(polyline.Count - 1);
                polyline.meshOutOfDate = true;
            }

            if (_drawnCells.Count <= 1)
            {
                tip.enabled = false;
                polyline.enabled = false;
            }
            else
            {
                var top = _drawnCells.Peek();
                
                tip.transform.position = top.transform.position;

                if (top.node)
                {
                    Vibrator.Vibrate(VibrationType.Rigid);
                }
                else
                {
                    Vibrator.Vibrate(VibrationType.Light);
                }
            }
        }

        private Color GetOccupiedColor()
        {
            return new Color(color.r, color.g, color.b, occupiedAlpha);
        }
    }
}