using System.Collections.Generic;
using Mechanics.RoboticFlows;
using Mechanics.RoboticFlows.Obstacles;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Game.CircuitBoard.Mechanics.RoboticFlows.Scripts.Behaviours.Obstacles
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class ObstacleWallBuilder : MonoBehaviour
    {

        

        [SerializeField] private Obstacle obstaclePrefab;

        [ReadOnly][SerializeField] private float xzBorderValue = 0.3f;

        [SerializeField] private DirectionType directionType;
        

        [FormerlySerializedAs("_obstacles")] [SerializeField]private Dictionary<DirectionType, Obstacle> obstacles = new();

        [HorizontalGroup("Add Wall")][Button(ButtonSizes.Large),GUIColor(0,1f,0)]
        public void Add()
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            if (obstacles.ContainsKey(directionType))
            {
                Debug.LogError("There is already a obstacle in left");
                return;
            }
            Selection.activeObject = PrefabUtility.InstantiatePrefab(obstaclePrefab, transform);
            var tempPrefab = Selection.activeGameObject;
            switch (directionType)
            {
                case DirectionType.Left:
                    position =  new Vector3(-xzBorderValue, 0, 0);
                    rotation = Quaternion.identity;
                    break;
                case DirectionType.Right:
                    position =  new Vector3(xzBorderValue, 0, 0);
                    rotation = Quaternion.identity;
                    break;
                case DirectionType.Up:
                    position =  new Vector3(0, 0, xzBorderValue);
                    rotation = Quaternion.Euler(0,90f,0);
                    break;
                case DirectionType.Down:
                    position =  new Vector3(0, 0, -xzBorderValue);
                    rotation = Quaternion.Euler(0,90f,0);
                    break;
            }
            

            tempPrefab.transform.localPosition = position;
            tempPrefab.transform.rotation = rotation;
            tempPrefab.GetComponent<Obstacle>().SetDirectionType(directionType);
            obstacles.Add(directionType,tempPrefab.GetComponent<Obstacle>());          
        }

        [HorizontalGroup("Remove Wall")][Button(ButtonSizes.Large),GUIColor(1f,0,0)]
        public void Remove()
        {
            var obstacle = obstacles[directionType];
            obstacles.Remove(directionType);
            DestroyImmediate(obstacle.gameObject);
        }

        [HorizontalGroup("Clear Data")][Button(ButtonSizes.Large),GUIColor(0f,0,1)]

        public void ClearData()
        {
            obstacles.Remove(directionType);
        }
   
    }
#endif
}