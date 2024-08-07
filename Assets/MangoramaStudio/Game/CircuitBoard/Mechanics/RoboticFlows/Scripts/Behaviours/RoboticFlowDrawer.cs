using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Game.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Systems.TutorialSystem.Scripts;
using Mechanics.Scripts;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class RoboticFlowDrawer : PlayableMechanicBehaviour, IShowHint
    {
        [SerializeField] private FlowGrid grid;
        [SerializeField] private RoboticFlowHint hint;
        [SerializeField] private RoboticFlowInputSurface surface;
        [SerializeField] private List<FlowDrawer> drawers;
        [SerializeField] private TutorialComponent tutorialComponent;
        public List<FlowDrawer> completedDrawers = new();
        
        private Camera _mainCamera;
        
        private Node _selectedNode;
        private FlowDrawer _selectedDrawer;

        public Action onRelease;
        public Action onClear;
        public Action onDraw;
        public Action onConnectNode;
        public Action onClearDisconnected;



        private EventManager _eventManager;

        public IReadOnlyList<FlowDrawer> Drawers => drawers;

        public FlowDrawer GetSelectedFlowDrawer() => _selectedDrawer;

 
        
        [Button]
        public override void Initialize()
        {
            _mainCamera = MainCamera.Instance.Camera;
            
            foreach (var drawer in drawers)
            {
                drawer.Initialize(this);
            }

            if (HasTutorialComponent())
            {
                tutorialComponent.SetPlayableCells(grid.Cells.ToList());
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
            _eventManager = GameManager.Instance.EventManager;
            Debug.LogWarning("Enable");
            base.Enable();
            surface.Pressed += SurfacePressed;
            surface.Dragged += SurfaceDragged;
            surface.Released += SurfaceReleased;
            _eventManager.OnUndo += RaiseUndo;
            _eventManager.OnAutoComplete += AutoComplete;
        }

        [Button]
        public override void Disable()
        {
            base.Disable();
            surface.Pressed -= SurfacePressed;
            surface.Dragged -= SurfaceDragged;
            surface.Released -= SurfaceReleased;
            _eventManager.OnUndo -= RaiseUndo;
            _eventManager.OnAutoComplete -= AutoComplete;

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

        public bool HasTutorialComponent()
        {
            return tutorialComponent != null;
        }
        
        public void SurfacePressed(Vector2 screenPosition)
        {
            if (CellRaycast(screenPosition, out var cell))
            {
                if (cell.IsOccupied)
                {
                    var occupiedNode = cell.GetOccupiedFlowDrawer().DrawnCells.FirstOrDefault(x => x.node);
                    if (occupiedNode!=null && occupiedNode.IsOccupied)
                    {
                        _selectedDrawer = occupiedNode.GetOccupiedFlowDrawer();
                        _eventManager.SelectOccupiedCell(_selectedDrawer);
                    }
                }
                
                
                if (!_selectedDrawer)
                {
                    if (cell.node)
                    {
                        SelectNode(cell.node);
                        ScaleUpNodes(cell.node.Id);     
                    }
                }
                

                if (cell.node)
                {
                   // _eventManager.ResetNoteIndexSound();
               

                   var matchedDrawer = drawers.Find(x => x.Id == cell.node.Id);
                   if (matchedDrawer.DrawnCells.Count>0)
                   {
                       matchedDrawer.Clear();
                       _eventManager.ResetFlow(_selectedDrawer);
                   }
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
                       // _selectedDrawer.Clear();
                        Remove(_selectedDrawer);
                        _selectedDrawer.DrawCell(cell);
                    }
                    else if (_selectedDrawer.DrawnCells.Peek() == cell)
                    {
                        _selectedDrawer.DrawCell(cell);
                    }
                    else
                    {
                        _selectedDrawer = null;
                        Remove(_selectedDrawer);
                        /*
                        _eventManager.ClearDisconnectedCell(_selectedDrawer);
                        _selectedDrawer.Clear();
                        onClearDisconnected?.Invoke();
                        */
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
            if (_selectedDrawer == null)
            {
                return false;
            }

            if (_selectedDrawer.CurrentCell == null)
            {
                return false;
            }
            
            var direction =_selectedDrawer.CurrentCell.DirectionAccordingToTargetCell(cell);
                
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
     
                }      
            }
            
            // all clear there is no obstacles in my path
            return false;
        }

        
        public void SurfaceDragged(Vector2 screenPosition)
        {
            if (CellRaycast(screenPosition, out var cell))
            {
                
                if (cell.IsOccupied)
                {
                    if (cell.GetOccupiedFlowDrawer() == _selectedDrawer)
                    {
                        var occupiedNode = cell.GetOccupiedFlowDrawer().DrawnCells.FirstOrDefault(x => x.node);
                        if (occupiedNode!=null && occupiedNode.IsOccupied)
                        {
                            SelectNode(occupiedNode.node);
                            _selectedDrawer = cell.GetOccupiedFlowDrawer();
                        }     
                    }
                    else
                    {
                        cell.GetOccupiedFlowDrawer().ClearToCell(cell);
                    }
                    
                   
                }
                
                if (!_selectedDrawer || (_selectedDrawer.DrawnCells.Count != 0 &&
                                         !_selectedDrawer.DrawnCells.Peek().IsNeighbor(cell)))
                {
                    return;
                }
                    
                
                if (ReachObstacles(cell))
                {
                    return;
                }
                
             
                
                
                if (_selectedDrawer.DrawnCells.Contains(cell))
                {
              
                    _eventManager.DecrementNoteIndexSound(_selectedDrawer.DrawnCells.Count-1);
                    _eventManager.PlayNoteSound();
                    _selectedDrawer.ClearToCell(cell);
                    _eventManager.ClearCell(_selectedDrawer);
                    onClear?.Invoke();
                   
                }
                else if (!_selectedDrawer.FlowComplete)
                {
                    
                    if (cell.IsOccupied)
                    {
                        var drawer = drawers.First(d => d.DrawnCells.Contains(cell));
                        if (cell.node)
                            return;
                        foreach (var selectedDrawerDrawnCell in drawer.DrawnCells)
                        {
                            selectedDrawerDrawnCell.SetDefaultColor();
                        }
                        
                        drawer.Clear();
                        Remove(drawer);
                    }
                    

                    _eventManager.PlayNoteSound();
                    _selectedDrawer.DrawCell(cell);
                    _eventManager.IncrementNoteIndexSound();
                    _eventManager.VibrateDrawCell();
                    _eventManager.DrawCell();

                    if (_selectedDrawer.FlowComplete)
                    {

                        if (HasTutorialComponent())
                        {
                            var def = tutorialComponent.GetCurrentTutorialDefinition;
                            if (def.flowDrawer == _selectedDrawer)
                            {
                                def.isCompleted = true;
                                tutorialComponent.TryIncrementTutorialIndex();
                                tutorialComponent.SetPlayableCells(grid.Cells.ToList());
                            }
                        }
                        BounceFlow(cell.node.Id);
                        Add();

                        var reverse = _selectedDrawer.DrawnCells.Reverse().ToList();
                        for (int i = 0; i < reverse.Count; i++)
                        {
                            reverse.ElementAt(i).PlayCompleteBlob(i);   
                        }
                        
                  
                        _eventManager.ConnectFlow(_selectedDrawer);    
                        _eventManager.CompleteFlow(_selectedDrawer,_selectedNode);
                        _selectedDrawer = null;
                        _selectedNode = null;
                        onConnectNode?.Invoke();
                       // _eventManager.ResetNoteIndexSound();
                        _eventManager.VibrateFlowComplete();
                        _eventManager.PlayFlowSuccessSound();
                       
                        
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

            _eventManager.ReleaseDrawing(_selectedDrawer);
            _selectedDrawer.BoingEffect();
            
            ScaleDownNodes(_selectedDrawer.Id);
            if (_selectedDrawer.DrawnCells.Count <= 1)
            {
                _selectedDrawer.Clear();
                Remove(_selectedDrawer);
                foreach (var selectedDrawerDrawnCell in _selectedDrawer.DrawnCells)
                {
                    selectedDrawerDrawnCell.SetDefaultColor();
                }

                _selectedDrawer = null;
                _selectedNode = null;
            }
            else if (_selectedDrawer.DrawnCells.Last().node is { } lastNode)
            {
                if (CellRaycast(screenPosition, out var cell) && cell.node && cell.node.Id == lastNode.Id)
                {
                   // _selectedDrawer.Clear();
                   // Remove(_selectedDrawer);
                    foreach (var selectedDrawerDrawnCell in _selectedDrawer.DrawnCells)
                    {
                        selectedDrawerDrawnCell.SetDefaultColor();
                    }

                    _selectedDrawer = null;
                    _selectedNode = null;
                     Remove(_selectedDrawer);
                }
                else
                {
                    var reverse = _selectedDrawer.DrawnCells.Reverse().ToList();
                    for (int i = 0; i < reverse.Count; i++)
                    {
                        reverse.ElementAt(i).PlayCompleteBlob(i);   
                    }
                    
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

            /*
            if (_selectedDrawer && _selectedDrawer != newDrawer)
            {
                if (_selectedDrawer.DrawnCells.Count<=0)
                {
                    _selectedDrawer.Clear();
                    Remove(_selectedDrawer);      
                }
            }
            */
              
            
            _selectedDrawer = newDrawer;

            /*
            if (_selectedNode && _selectedNode != node)
            {
                _selectedDrawer.Clear();
                Remove(_selectedDrawer);
            }
            */
             
            
            _selectedNode = node;
        }

        private void BounceFlow(int id)
        {
            var cells = grid.Cells.Where(c => c.node && c.node.Id == id);

            foreach (var cell in cells)
            {
                cell.node.Bounce();  
            }
        }
        
        private void ScaleUpNodes(int id)
        {
            var cells = grid.Cells.Where(c => c.node && c.node.Id == id);

            foreach (var cell in cells)
            {
                cell.node.ScaleUp();  
            }
        }
        
        private void ScaleDownNodes(int id)
        {
            var cells = grid.Cells.Where(c => c.node && c.node.Id == id);

            foreach (var cell in cells)
            {
                cell.node.ScaleDown();  
            }
        }

        private IEnumerator CRCheck()
        {
            yield return new WaitUntil(() => grid.Cells.All(x => x.isColorAnimCompleted));
            yield return new WaitForSeconds(.75f);
            _eventManager.CompleteAllFlows();
            _eventManager.ResetNoteIndexSound();
            _eventManager.VibrateLevelComplete();
            _eventManager.PlayLevelSuccessSound();
            RaiseSuccess();
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

                StartCoroutine(CRCheck());
            }
            else if (drawers.All(d => d.FlowComplete))
            {
              
                RaiseWarning();
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

        #region Restart

        public override void RaiseRestart()
        {
            base.RaiseRestart();
            completedDrawers.Clear();
        }


        #endregion

        #region Undo
        
        protected override void RaiseUndo()
        {
            base.RaiseUndo();
            if (completedDrawers.Count<=0)
            {
                Debug.LogError("no completed drawers");
                return;
            }
            var last = completedDrawers.Last();
            last.Clear();
            Remove(last);
        }
        
        private void Add()
        {
            if (!completedDrawers.Contains(_selectedDrawer))
            {
                completedDrawers.Add(_selectedDrawer);       
            }
        }

        private void Remove(FlowDrawer drawer)
        {
            if (completedDrawers.Contains(drawer))
            {
                completedDrawers.Remove(drawer);       
            }
        }
        #endregion
        
        protected override void AutoComplete()
        {
            base.AutoComplete();
            drawers.ForEach(x=>x.AutoComplete());
            CheckAndComplete();         
            
        }
    }
}