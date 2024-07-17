using System;
using UnityEngine;

namespace Mechanics.RoboticFlows.Obstacles
{
    public class Obstacle : MonoBehaviour,IObstacle
    {
        [SerializeField] private DirectionType directionType;
        public DirectionType DirectionType => directionType;
        
        /// <summary>
        /// Opposite direction is needed for handling block operations when the current selected cell has obstacle
        /// </summary>
        public DirectionType OppositeDirectionType { get; private set; }

        private void Awake()
        {
            SetOppositeDirection();
        }

        private void SetOppositeDirection()
        {
            OppositeDirectionType = DirectionType switch
            {
                DirectionType.Down => DirectionType.Up,
                DirectionType.Up => DirectionType.Down,
                DirectionType.Left => DirectionType.Right,
                DirectionType.Right => DirectionType.Left,
                _ => OppositeDirectionType
            };
        }

        public void Block()
        {
            Debug.LogError("Blocked");
        }
    }
}