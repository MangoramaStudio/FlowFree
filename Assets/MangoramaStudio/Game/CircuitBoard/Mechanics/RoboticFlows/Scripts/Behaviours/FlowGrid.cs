using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Game.Scripts.Behaviours;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class FlowGrid : MonoBehaviour
    {
        [SerializeField] private float orthoSize;
        
        public bool HasObstacles => _cells.Any(x => x.HasObstacles);
        
        public void SetOrthoSize(float size)
        {
            orthoSize = size;
        }
        
        private Cell[] _cells;

        public bool Ready { get; private set; }
        public IReadOnlyCollection<Cell> Cells => _cells;

        private void Awake()
        {
            MainCamera.Instance.Camera.orthographicSize = orthoSize;
            _cells = GetComponentsInChildren<Cell>();
        }

        public void AnimateIn()
        {
            Ready = false;

            var descendingOrder = _cells.OrderByDescending(c => c.y).ToList();
            var highest = descendingOrder.First().y;

            for (var index = 0; index < descendingOrder.Count; index++)
            {
                var cell = descendingOrder[index];

                if (index == descendingOrder.Count - 1)
                {
                    cell.AnimateIn(0.25f, (highest - cell.y) * 0.125f ,() =>
                    {
                        Ready = true;
                    });
                }
                else
                {
                    cell.AnimateIn(0.25f, (highest - cell.y) * 0.125f);
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            return _cells.First(c => c.x == x && c.y == y);
        }

        public Node GetNode(int x, int y)
        {
            return GetCell(x, y).node;
        }

        public Vector2Int GetNodeCoordinate(Node node)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                var cell = _cells[i];

                if (cell.node == node)
                {
                    return new Vector2Int(cell.x, cell.y);
                }
            }

            return new Vector2Int(-1, -1);
        }
    }
}