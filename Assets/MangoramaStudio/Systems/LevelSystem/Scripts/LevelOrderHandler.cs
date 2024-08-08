using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Systems.LevelSystem.Scripts
{
    public class LevelOrderHandler : MonoBehaviour
    {
        [SerializeField] private LevelOrderConfig levelOrderConfig;
        
        
        public void Initialize()
        {
            levelOrderConfig.InitializeConfig();
        }

        public List<int> GetCurrentLevelOrder()
        {
            return levelOrderConfig.DisplayLevelDataList;
        }
        
        public List<int> GetCurrentLoopLevelOrder()
        {
            return levelOrderConfig.DisplayLoopLevelDataList;
        }
       
    }
}