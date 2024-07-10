using MatchinghamGames.Handyman;
using UnityEngine;

namespace MangoramaStudio.Game.Scripts.Behaviours
{
    public class MainCamera : SingletonBehaviour<MainCamera>
    {
        [SerializeField] private Camera cam;

        public Camera Camera => cam;
    }
}