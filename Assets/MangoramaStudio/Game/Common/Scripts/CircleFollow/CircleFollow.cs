using System;
using DG.Tweening;
using MangoramaStudio.Game.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;

namespace MangoramaStudio.Game.Common.Scripts.CircleFollow
{
    using UnityEngine;

    public class CircleFollow : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer smallCircleRenderer,fingerCircleRenderer;
        [SerializeField] private FlowDrawer drawer;
        [SerializeField] private float alpha;

        private bool _canFollow;
        private void Start()
        {
            fingerCircleRenderer.color = smallCircleRenderer.color;
            fingerCircleRenderer.DOFade(alpha, 0f);
            ToggleEvents(true);
        }

        private void OnDestroy()
        {
            ToggleEvents(false);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.Instance.EventManager;
            if (isToggled)
            {
                eventManager.OnSelectOccupiedCell += StartCircle;
                eventManager.OnDrawCell += StartCircle;
                eventManager.OnReleaseDrawing += StopCircle;
                eventManager.OnConnectFlow += StopCircle;
            }
            else
            {
                eventManager.OnSelectOccupiedCell -= StartCircle;
                eventManager.OnDrawCell -= StartCircle;
                eventManager.OnReleaseDrawing -= StopCircle;
                eventManager.OnConnectFlow -= StopCircle;
            }
        }

        private void StopCircle(FlowDrawer flowDrawer)
        {
            fingerCircleRenderer.enabled = false;

            if (flowDrawer == drawer)
            {
                _canFollow = false;
            }
        }

        private void StartCircle(FlowDrawer flowDrawer)
        {
         
            if (drawer == flowDrawer)
            {
                fingerCircleRenderer.enabled = true;
                _canFollow = true;
            }   
        }

        private void Update()
        {
            
            if (drawer == null)
            {
                return;
            }

            if (!_canFollow)
            {
                return;
            }
            
            
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = MainCamera.Instance.Camera.ScreenToWorldPoint(mousePosition);
            worldPosition.y = 0;
            transform.position = worldPosition;
        }
    }

}