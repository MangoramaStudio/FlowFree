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

#if UNITY_EDITOR
        [Button]
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


        [Button]
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
        
#endif
       
    }
}