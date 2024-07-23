using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangoramaStudio.Scripts.Behaviours;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    [CreateAssetMenu(fileName = "FlowConverterConfig", menuName = "Game/FlowConverterConfig", order = 0)] 
    public class FlowConverterConfig : ScriptableObject
    {
        public List<LevelBehaviour> levelBehaviours = new();

        public List<LevelSizeCategoryDefinition> levelSizeCategoryDefinitions = new();

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium)]
        public void SetTileAccordingToColors()
        {
            for (int i = 0; i < levelBehaviours.Count; i++)
            {
                levelBehaviours[i].GetComponent<FlowDrawerConverter>().Convert();
                EditorUtility.SetDirty(levelBehaviours[i].gameObject);
                PrefabUtility.SavePrefabAsset(levelBehaviours[i].gameObject);
            }
            
            Debug.Log("All done");
        }


        [Button(ButtonSizes.Medium)]
        public void SearchAnyUnset()
        {
            for (int i = 0; i < levelBehaviours.Count; i++)
            {
                var drawers = levelBehaviours[i].GetComponentsInChildren<FlowDrawer>().ToList();
                var any = drawers.Any(x => x.TileSprite == null);
                if (any)
                {
                    Debug.LogError($"level {i} has not a tile sprite");
                }
            }   
        }

        [Button(ButtonSizes.Large)]
        public void CategorizeLevelsAccordingToSizes()
        {
            
            for (int i = 0; i < levelBehaviours.Count; i++)
            {
                var level = levelBehaviours[i];
                var builder = level.GetComponentInChildren<RoboticFlowBuilder>();
                var size = builder.GetSize();
                var requiredDefinition = levelSizeCategoryDefinitions.Find(x => x.size == size);
                if (requiredDefinition!=null)
                {
                    requiredDefinition.levels.Add(level);
                }
            }
        }
        
#endif
       
    }

    [Serializable]
    public class LevelSizeCategoryDefinition
    {
        [BoxGroup]public float cameraOrthoSize;
        public Vector2Int size;
        public List<LevelBehaviour> levels = new();
#if UNITY_EDITOR
        [Button(ButtonSizes.Medium),GUIColor(0,1,0)]
        public void SetCameraOrthoSize()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                var level = levels[i];
                var flowGrid = level.GetComponentInChildren<FlowGrid>();
                flowGrid.SetOrthoSize(cameraOrthoSize);
                EditorUtility.SetDirty(level.gameObject);
                PrefabUtility.SavePrefabAsset(level.gameObject);
                Debug.LogError(flowGrid.gameObject);
            }
        }
#endif
    }
}