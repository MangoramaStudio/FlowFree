using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Scripts.Data;
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

        public List<string> GetCurrentLevelOrder()
        {
            return levelOrderConfig.LevelOrder;
        }
    }
}