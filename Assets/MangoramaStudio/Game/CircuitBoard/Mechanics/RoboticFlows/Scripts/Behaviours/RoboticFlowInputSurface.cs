using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class RoboticFlowInputSurface : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action<Vector2> Pressed;
        public event Action<Vector2> Dragged;
        public event Action<Vector2> Released;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Down");
            Pressed?.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Drag");
            Dragged?.Invoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Up");
            Released?.Invoke(eventData.position);
        }
    }
}