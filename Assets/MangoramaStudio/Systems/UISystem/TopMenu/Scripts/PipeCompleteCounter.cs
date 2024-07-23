
using System;
using System.Linq;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Scripts.Managers.Buttons;
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
                flowDrawer.onClearDisconnected += CalculateCompletedPipe;

            }
            else
            {
                flowDrawer.onDraw -= CalculateCompletedPipe;
                flowDrawer.onClear -= CalculateCompletedPipe;
                flowDrawer.onRelease -= CalculateCompletedPipe;
                flowDrawer.onClearDisconnected -= CalculateCompletedPipe;

            }
        }

        public async void Undo()
        {
            try
            {
                await Task.Yield();
                CalculateCompletedPipe();
            }
            catch (Exception e)
            {
                 Debug.LogError(e);
            }
        }

        public void Restart()
        {
            CompletedPipeProportion = 0;
        }

        private void CalculateCompletedPipe()
        {
            var currentOccupiedCellsCount = flowGrid.Cells.Count(x => x.IsOccupied);
            CompletedPipeProportion = ((float)currentOccupiedCellsCount / (float)_totalPipeCount) *100f;
            var pipeComplete = Mathf.CeilToInt(CompletedPipeProportion);
            onCompletePipe?.Invoke(pipeComplete);
        }
    }
}