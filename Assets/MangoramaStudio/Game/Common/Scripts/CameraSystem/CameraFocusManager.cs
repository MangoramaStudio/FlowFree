using System.Collections.Generic;
using System.Linq;
using Behaviours;
using MangoramaStudio.Game.Scripts.Behaviours;
using MatchinghamGames.Handyman;
using UnityEngine;

namespace Managers
{
    public class CameraFocusManager : SingletonBehaviour<CameraFocusManager>
    {
        private List<CameraFocus> _focusList = new List<CameraFocus>();

        private Camera _camera => MainCamera.Instance.Camera;

        private void Update()
        {
            if (_camera == null)
            {
                return;
            }
            _focusList.Sort(new FocusPriorityComparer());
            var focus = _focusList.FirstOrDefault();
            
            if (focus)
            {
                var mainCameraPosition = _camera.transform.position;
                var focalTransformPosition = focus.FocalTransform.position;
                
                _camera.transform.position = new Vector3(focalTransformPosition.x, mainCameraPosition.y, focalTransformPosition.z);
            }
        }

        public void Register(CameraFocus focus)
        {
            _focusList.Add(focus);
        }

        public void Unregister(CameraFocus focus)
        {
            _focusList.Remove(focus);
        }

        
        private class FocusPriorityComparer : IComparer<CameraFocus>
        {
            public int Compare(CameraFocus x, CameraFocus y)
            {
                var xPriority = x == null ? 0 : x.FocusPriority;
                var yPriority = y == null ? 0 : y.FocusPriority;
                return yPriority - xPriority;
            }
        }
    }
}