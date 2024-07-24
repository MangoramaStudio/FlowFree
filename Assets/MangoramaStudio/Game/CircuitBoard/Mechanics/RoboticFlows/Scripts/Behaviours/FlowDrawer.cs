using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchinghamGames.VibrationModule;
using Shapes;
using Sirenix.OdinInspector;
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

        [SerializeField] private Sprite tileSprite;
        private Stack<Cell> _drawnCells;
        
        public int Id => id;

        public Sprite TileSprite => tileSprite;

        public bool FlowComplete => _drawnCells.Count(c => c.node) == 2;

        public Stack<Cell> DrawnCells => _drawnCells;

        public Cell CurrentCell => DrawnCells.Count <=0 ? null : DrawnCells?.Peek();

        public Polyline Polyline => polyline;

        public Color GetColor() => color;

        private RoboticFlowDrawer _roboticFlowDrawer;
        
        public void Initialize(RoboticFlowDrawer roboticFlowDrawer)
        {
            _roboticFlowDrawer = roboticFlowDrawer;
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

        public List<Cell> a = new();

        public RoboticFlowHint hint;
        [Button]
        public void AutoComplete()
        {
            polyline.points.Clear();
            var cells = hint.GetHints(id);
            var nodes = cells.FindAll(x => x.node != null).ToList();

            
            List<Vector2Int> path = FindPath(new Vector2Int(nodes[0].x,nodes[0].y), new Vector2Int(nodes[1].x,nodes[1].y),cells);


            for (int i = 0; i < path.Count; i++)
            {
                var p = path[i];
               var req = cells.Find(x => x.x == p.x && x.y == p.y);
               if (req!=null)
               {
                   a.Add(req);
               }
            }
            
            
            if (path != null)
            {
                foreach (Vector2Int position in path)
                {
                    Debug.Log(position);
                }
            }
           
        }
        
        List<Vector2Int> FindPath(Vector2Int start, Vector2Int end,List<Cell> cellList)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);

            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            cameFrom[start] = start;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == end)
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (Vector2Int next in GetNeighbors(current))
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        var any = cellList.Any(x => x.x == next.x && x.y == next.y);
                        if (any)
                        {
                            queue.Enqueue(next);
                            cameFrom[next] = current;      
                        }
                    }
                }
            }

            return null;
        }

        List<Vector2Int> GetNeighbors(Vector2Int node)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1), // yukarı
                new Vector2Int(1, 0), // sağ
                new Vector2Int(0, -1), // aşağı
                new Vector2Int(-1, 0) // sol
            };

            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighbor = node + direction;
                if (neighbor.x >= 0 && neighbor.x <5 && neighbor.y >= 0 && neighbor.y < 5)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (current != cameFrom[current])
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(current);
            path.Reverse();
            return path;
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

        public void SetTileSprite(Sprite sprite)
        {
            tileSprite = sprite;
        }

        private Color GetOccupiedColor()
        {
            return new Color(color.r, color.g, color.b, occupiedAlpha);
        }
    }
}