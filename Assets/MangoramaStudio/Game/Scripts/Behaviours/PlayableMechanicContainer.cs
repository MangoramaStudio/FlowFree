using Mechanics.Scripts;
using UnityEngine;

namespace Behaviours
{
    public class PlayableMechanicContainer : MonoBehaviour
    {
        [SerializeField] private PlayableMechanicBehaviour playable;

        public PlayableMechanicBehaviour Playable => playable;
    }
}