using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using UnityEngine;

namespace MangoramaStudio.Game.Test
{
    public class LineClipTracker : MonoBehaviour
    {

        [SerializeField] private float startDelay;
        [SerializeField]private LineRenderer lineRenderer;
        [SerializeField] private Node node;
        [SerializeField] private float speed = 0.25f;
        
        public static readonly int ClipRight = Shader.PropertyToID(ClipUvRightID);
        public static readonly int ClipLeft = Shader.PropertyToID(ClipUvLeftID);
        
        private const string ClipUvRightID = "_ClipUvRight";
        private const string ClipUvLeftID = "_ClipUvLeft";

        private const string ClippingKeyword = "CLIPPING_ON";
        
        public List<Cell> positions = new();

        private bool _killTrailCalled;
        private float _initialTime;
        private Sequence _flowSequence;
        private LineRenderer _instantiatedLineRenderer;

        
        private void Start()
        {
            ToggleEvents(true);
            var mat = lineRenderer.material;
            var newMat =Instantiate(mat);
            lineRenderer.material = newMat;
        }
        
        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void OnDestroy()
        {
            ToggleEvents(false);
        }
        
        private void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.Instance.EventManager;
            if (isToggled)
            {
                eventManager.OnCompleteFlow += GetCellsList;
                eventManager.OnClearCell += ClearTrail;
                eventManager.OnClearDisconnectedCell += ClearTrail;
                eventManager.OnResetFlow += ClearTrail;
                eventManager.OnRestartLevel += RestartTrail;
                eventManager.OnDrawCell += StartDrawCell;
                eventManager.OnSelectOccupiedCell += ClearTrail;

            }
            else
            {
                eventManager.OnCompleteFlow -= GetCellsList;
                eventManager.OnClearCell -= ClearTrail;
                eventManager.OnClearDisconnectedCell -= ClearTrail;
                eventManager.OnResetFlow -= ClearTrail;
                eventManager.OnRestartLevel -= RestartTrail;
                eventManager.OnDrawCell -= StartDrawCell;
                eventManager.OnSelectOccupiedCell -= ClearTrail;

            }
        }

        private void StartDrawCell()
        {
            _isClear = false;
        }

        private void RestartTrail()
        {
            KillSequence();
            ResetTrail();
        }

        private bool _isClear;

        private void ClearTrail(FlowDrawer flowDrawer)
            {
                if (IsMatchedWithDrawer(flowDrawer))
                {
                    KillSequence();
                    ResetTrail();
                    _isClear = true;
                }
            }
        
            private void ResetTrail()
            {
                lineRenderer.gameObject.SetActive(false);
                lineRenderer.positionCount = 0;
                ResetClipping();
            }
            
            private async void GetCellsList(FlowDrawer flowDrawer,Node selectedNode)
            {

                await Task.Delay(TimeSpan.FromSeconds(startDelay));
                if (_isClear)
                {
                    return;
                }
                if (IsMatchedWithDrawer(flowDrawer))
                {
                    ResetTrail();
                    positions.Clear();
                    positions = flowDrawer.DrawnCells.ToList();
                    if (node == selectedNode)
                    {
                        var cell = node.GetComponentInParent<Cell>();
                        var cellPosition = new Vector2Int(cell.x, cell.y);
                        var firstCellPosition = new Vector2Int(positions[0].x, positions[0].y);
                        if (cellPosition != firstCellPosition)
                        {
                            positions.Reverse();
                        }
                    }
                    
                    GetLineRenderer();               
                    InstantiateCloneLine();
                }
            }

            
            private void GetLineRenderer()
            {
                lineRenderer.gameObject.SetActive(true);
                var controller = node.GetComponentInChildren<LineRendererController>();
                if (controller == null)
                {
                    return;
                }
                var original = node.GetComponentInChildren<LineRendererController>().GetLineRenderer();
               
                lineRenderer.positionCount = original.positionCount;
                var localPosition = original.transform.localPosition;
                lineRenderer.transform.localPosition = new Vector3(localPosition.x, localPosition.y + 0.1f, localPosition.z); lineRenderer.transform.localRotation = original.transform.localRotation;
                
                for (int i = 0; i < original.positionCount; i++)
                {
                    lineRenderer.SetPosition(i, original.GetPosition(i));
                }
            }

            private void InstantiateCloneLine()
            {
                ResetClipping();
             
                var p = Proportion();
                DOVirtual.Float(0f, 1f, speed * positions.Count, (x) =>
                {
                    lineRenderer.material.SetFloat(ClipLeft,x);
                    lineRenderer.material.SetFloat(ClipRight,(1f-p) -x);
                });
            }

            private void ResetClipping()
            {
                lineRenderer.material.SetFloat(ClipLeft,0);
                lineRenderer.material.SetFloat(ClipRight,1f);
            }

            private const int MIN_COUNT = 3;
            private const float DIFF = 0.2f;
            private float Proportion()
            {
                var prop = (MIN_COUNT * DIFF) / positions.Count();
                return prop;
            }
        
            private bool IsMatchedWithDrawer(FlowDrawer flowDrawer)
            {
                return node.Id == flowDrawer.Id;
            }

        
            private void KillSequence()
            {
                _flowSequence?.Kill();   
            }

          
    }
}