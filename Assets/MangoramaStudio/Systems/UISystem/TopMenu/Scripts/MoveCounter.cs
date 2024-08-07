using System;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class MoveCounter : MonoBehaviour
    {
        [SerializeField] private RoboticFlowDrawer flowDrawer;

        public int MoveCount() => _moveCount;
        
        private int _moveCount;
        public Action<int> onMoveCountUpdate;

        private FlowDrawer _lastDrawer;
        
        private void Start()
        {
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
                GameManager.Instance.EventManager.OnReleaseDrawing+=CheckDrawing;
                GameManager.Instance.EventManager.OnResetFlow += CheckDrawing;
                GameManager.Instance.EventManager.OnConnectFlow += CheckDrawing;
                //flowDrawer.onConnectNode += ConnectNode;  
            }
            else
            {
                GameManager.Instance.EventManager.OnReleaseDrawing-=CheckDrawing;
                GameManager.Instance.EventManager.OnResetFlow -= CheckDrawing;
                GameManager.Instance.EventManager.OnConnectFlow -= CheckDrawing;
                //flowDrawer.onConnectNode -= ConnectNode;  
            }
        }

        private void CheckDrawing(FlowDrawer drawer)
        {
            if (_lastDrawer == null)
            {
                _lastDrawer = drawer;
                ConnectNode();
                return;
            }

            if (_lastDrawer == drawer) return;
            ConnectNode();
            _lastDrawer = drawer;
        }

        public void Restart()
        {
            _moveCount = 0;
        }

        public async void Undo()
        {
            try
            {
                await Task.Yield();
                _moveCount--;
                if (_moveCount<=0)
                {
                    _moveCount = 0;
                }
                onMoveCountUpdate?.Invoke(_moveCount);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void ConnectNode()
        {
            _moveCount++;
            onMoveCountUpdate?.Invoke(_moveCount);
        }
    }
}