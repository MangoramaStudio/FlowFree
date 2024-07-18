
using System;
using System.Linq;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class PipeCompleteCounter : MonoBehaviour
    {
        [SerializeField] private RoboticFlowDrawer flowDrawer;
        [SerializeField] private FlowGrid flowGrid;

        private int _totalPipeCount;
        private float CompletedPipeProportion{ get; set; }

        public Action<int> onCompletePipe;

        private void Start()
        {
            _totalPipeCount = flowGrid.Cells.Count;
            ToggleEvents(true);
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
            if (isToggled)
            {
                flowDrawer.onDraw += CalculateCompletedPipe;   
                flowDrawer.onClear += CalculateCompletedPipe;
                flowDrawer.onRelease += CalculateCompletedPipe;

            }
            else
            {
                flowDrawer.onDraw -= CalculateCompletedPipe;
                flowDrawer.onClear -= CalculateCompletedPipe;
                flowDrawer.onRelease -= CalculateCompletedPipe;


            }
        }

        private void CalculateCompletedPipe()
        {
            var currentOccupiedCellsCount = flowGrid.Cells.Count(x => x.IsOccupied);
            CompletedPipeProportion = ((float)currentOccupiedCellsCount / (float)_totalPipeCount) *100f;
            var pipeComplete = Mathf.CeilToInt(CompletedPipeProportion);
            //Debug.LogError(Mathf.CeilToInt(CompletedPipeProportion));
            onCompletePipe?.Invoke(pipeComplete);
        }
    }
}