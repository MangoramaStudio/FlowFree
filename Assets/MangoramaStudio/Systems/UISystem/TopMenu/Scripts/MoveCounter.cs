using System;
using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class MoveCounter : MonoBehaviour
    {
        [SerializeField] private RoboticFlowDrawer flowDrawer;

        private int _moveCount;
        public Action<int> onMoveCountUpdate;

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
                flowDrawer.onConnectNode += ConnectNode;  

            }
            else
            {
                flowDrawer.onDraw -= ConnectNode;

            }
        }

        public void Restart()
        {
            _moveCount = 0;
        }

        private void ConnectNode()
        {
            _moveCount++;
            onMoveCountUpdate?.Invoke(_moveCount);
        }
    }
}