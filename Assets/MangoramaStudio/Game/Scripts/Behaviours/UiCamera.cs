using MatchinghamGames.Handyman;
using UnityEngine;

namespace Behaviours
{
    public class UiCamera : SingletonBehaviour<UiCamera>
    {
        [SerializeField] private Camera cam;

        public Camera Camera => cam;
    }
}