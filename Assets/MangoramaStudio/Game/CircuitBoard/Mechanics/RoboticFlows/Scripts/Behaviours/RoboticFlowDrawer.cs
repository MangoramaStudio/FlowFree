using System;
using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Game.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.ApolloModule;
using MatchinghamGames.VibrationModule;
using Mechanics.RoboticFlows.Obstacles;
using Mechanics.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class RoboticFlowDrawer : PlayableMechanicBehaviour, IShowHint
    {
        [SerializeField] private FlowGrid grid;
        [SerializeField] private RoboticFlowHint hint;
        [SerializeField] private RoboticFlowInputSurface surface;
        [SerializeField] private List<FlowDrawer> drawers;

       // [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string singleMatchSfx;
       // [BoxGroup("SFX"), ValueDropdown(nameof(GetSfxKeys)), SerializeField] private string fullMatchSfx;

       
        private Camera _mainCamera;
        
        private Node _selectedNode;
        private FlowDrawer _selectedDrawer;

        public Action onRelease;
        public Action onClear;
        public Action onDraw;
        public Action onConnectNode;
        public Action onClearDisconnected;
       

        public IReadOnlyList<FlowDrawer> Drawers => drawers;

        public FlowDrawer GetSelectedFlowDrawer() => _selectedDrawer;
        
        [Button]
        public override void Initialize()
        {
            _mainCamera = MainCamera.Instance.Camera;
            
            foreach (var drawer in drawers)
            {
                drawer.Initialize();
            }
        }

        [Button]
        public override void Prepare()
        {
            grid.AnimateIn();
            
            foreach (var drawer in drawers)
            {
                drawer.Prepare();
            }
        }

        [Button]
        public override void Enable()
        {
            Debug.LogWarning("Enable");
            base.Enable();
            surface.Pressed += SurfacePressed;
            surface.Dragged += SurfaceDragged;
            surface.Released += SurfaceReleased;
        }

        [Button]
        public override void Disable()
        {
            base.Disable();
            surface.Pressed -= SurfacePressed;
            surface.Dragged -= SurfaceDragged;
            surface.Released -= SurfaceReleased;
        }

        [Button]
        public override void Clear()
        {
            base.Clear();

            foreach (var drawer in drawers)
            {
                drawer.Clear();
            }

            foreach (var cell in grid.Cells)
            {
                cell.ShowFillHint(false);
            }
        }

        public override void Dispose()
        {
            surface.Pressed -= SurfacePressed;
            surface.Dragged -= SurfaceDragged;
            surface.Released -= SurfaceReleased;
         
        }
        
        [Button]
        public void ShowHint()
        {
            hint.HighlightHint();
        }

        public void SurfacePressed(Vector2 screenPosition)
        {
            if (CellRaycast(screenPosition, out var cell))
            {
                if (!_selectedDrawer && cell.node)
                {
                    SelectNode(cell.node);
                }

                if (cell.node)
                {
                    BounceFlow(cell.node.Id);
                }

                if (!_selectedDrawer)
                {
                    return;
                }
                
                hint.StopHighlight();

                if (_selectedDrawer.DrawnCells.Contains(cell))
                {
                    _selectedDrawer.ClearToCell(cell);
                }
                else
                {
                    if (_selectedDrawer.DrawnCells.Count == 0)
                    {
                        _selectedDrawer.Clear();
                        _selectedDrawer.DrawCell(cell);
                    }
                    else if (_selectedDrawer.DrawnCells.Peek() == cell)
                    {
                        _selectedDrawer.DrawCell(cell);
                    }
                    else
                    {
                        _selectedDrawer.Clear();
                        _selectedDrawer = null;
                        onClearDisconnected?.Invoke();
                    }
                }
                
                RaiseAttempt();
            }
        }

        
        /// <summary>
        /// Check the next movement can be blocked by obstacles or not 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool ReachObstacles(Cell cell)
        {
            var direction =_selectedDrawer.CurrentCell.DirectionAccordingToTargetCell(cell);
            Debug.LogError(direction);
                
            if(_selectedDrawer.CurrentCell.HasObstacles)
            {
             
                // check if current cell has obstacles and my direction matches with it
                var requiredObstacle = _selectedDrawer.CurrentCell.Obstacles.Find(x => x.OppositeDirectionType == direction);
                if (requiredObstacle!=null)
                {
                    // yes
                    requiredObstacle.Block();
                    return true;
                }

                // no but also next cell i go to has obstacle and my direction matches with it
                var targetCellObstacle = cell.Obstacles.Find(x => x.DirectionType == direction);
                if (targetCellObstacle!=null)
                {
                    // yes
                    targetCellObstacle.Block();
                    return true;
                }
            }
            else
            {
                // checked my current cell has no obstacle and ı will check next cell according to my direction
                if (cell.HasObstacles)
                {
                    var requiredObstacle = cell.Obstacles.Find(x => x.DirectionType == direction);
                    if (requiredObstacle != null)
                    {
                        // yes
                        requiredObstacle.Block();
                        return true;
                    }
                    
                    /*
                    // also check opposite way 
                    var oppositeObstacle = cell.Obstacles.Find(x=>x.OppositeDirectionType == direction);
                    if (oppositeObstacle != null)
                    {
                        Debug.LogError("11");
                        //yes
                        oppositeObstacle.Block();
                        return true;
                    }
                    */
                }      
            }
            
            // all clear there is no obstacles in my path
            return false;
        }

        public void SurfaceDragged(Vector2 screenPosition)
        {
            if (CellRaycast(screenPosition, out var cell))
            {
                if (!_selectedDrawer || (_selectedDrawer.DrawnCells.Count != 0 && !_selectedDrawer.DrawnCells.Peek().IsNeighbor(cell)))
                    return;
                
                if (ReachObstacles(cell))
                {
                    return;
                }
                
                if (_selectedDrawer.DrawnCells.Contains(cell))
                {
                    _selectedDrawer.ClearToCell(cell);
                    onClear?.Invoke();
                }
                else if (!_selectedDrawer.FlowComplete)
                {
                    if (cell.IsOccupied)
                    {
                        var drawer = drawers.First(d => d.DrawnCells.Contains(cell));
                        if (cell.node)
                            return;
                        drawer.Clear();
                    }
                    
                    _selectedDrawer.DrawCell(cell);

                    if (_selectedDrawer.FlowComplete)
                    {
                        BounceFlow(cell.node.Id);
                       // Apollo.PlaySingleAudio(singleMatchSfx);
                        _selectedDrawer = null;
                        _selectedNode = null;
                        onConnectNode?.Invoke();
                    }
                    
                    CheckAndComplete();
                  
                }
                
                onDraw?.Invoke();
            }
        }

        private void SurfaceReleased(Vector2 screenPosition)
        {
            if (!_selectedDrawer)
                return;

            if (_selectedDrawer.DrawnCells.Count <= 1)
            {
                _selectedDrawer.Clear();
                _selectedDrawer = null;
                _selectedNode = null;
            }
            else if (_selectedDrawer.DrawnCells.Last().node is { } lastNode)
            {
                if (CellRaycast(screenPosition, out var cell) && cell.node && cell.node.Id == lastNode.Id)
                {
                    _selectedDrawer.Clear();
                    _selectedDrawer = null;
                    _selectedNode = null;
                }
            }
            
            onRelease?.Invoke();
        }

        public void SetDrawers(IEnumerable<FlowDrawer> list)
        {
            drawers = list.ToList();
        }

        private void SelectNode(Node node)
        {
            var newDrawer = drawers.First(d => d.Id == node.Id);
            
            if (_selectedDrawer && _selectedDrawer != newDrawer)
                _selectedDrawer.Clear();
            
            _selectedDrawer = newDrawer;
            
            if (_selectedNode && _selectedNode != node)
                _selectedDrawer.Clear();
            
            _selectedNode = node;
        }

        private void BounceFlow(int id)
        {
            var cells = grid.Cells.Where(c => c.node && c.node.Id == id);

            foreach (var cell in cells)
                cell.node.Bounce();
        }

        private void CheckAndComplete()
        {
            if (grid.Cells.All(c => c.IsOccupied))
            {
                foreach (var cell in grid.Cells)
                {
                    if (cell.node)
                        cell.node.Bounce();
                }
                
                Vibrator.VibratePreset(VibrationPreset.Success);
                //Apollo.PlaySingleAudio(fullMatchSfx);
                RaiseSuccess();
            }
            else if (drawers.All(d => d.FlowComplete))
            {
              
                RaiseWarning();
                //OverlaySystem.Instance.Push(nameof(CircuitBoardWarningOverlay));
            }
        }

        private bool CellRaycast(Vector2 screenPosition, out Cell cell)
        {
            var position = new Vector3(screenPosition.x, screenPosition.y, 50);
            var ray = _mainCamera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out var hit, 100f))
            {
                if (hit.collider.TryGetComponent(out cell))
                    return true;
            }

            cell = default;
            return false;
        }


        /*
        private IList<ValueDropdownItem<string>> GetSfxKeys()
        {
            return SfxUtility.GetSfxKeys();
        }
        */
    }
}