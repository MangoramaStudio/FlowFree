using System.Collections.Generic;
using MatchinghamGames.MeteorModule.Attributes;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MangoramaStudio.Systems.LevelSystem.Scripts
{
    [CreateAssetMenu(fileName = "LevelOrderConfig", menuName = "Config/LevelOrderConfig", order = 0)]
    public class LevelOrderConfig : ScriptableObject
    {

        public void InitializeConfig()
        {
            ParseJsonToLevelData(levelOrder);
        }
        
        [SerializeField] private List<string> levelOrderList = new();
        
        //[RemoteSetting("levelOrder")]
        [SerializeField] private string levelOrder;
        

        public List<string> LevelOrder => JsonConvert.DeserializeObject<List<string>>(levelOrder);
        
        
        [InfoBox("This fills the string with the level data struct you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large),GUIColor(0,1,1)]
        private void ConvertLevelDataToJson()
        {
            levelOrder = JsonConvert.SerializeObject(levelOrderList);
        }
        
        [InfoBox("This fills the level data struct list with the json string you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large)]
        public void ParseJsonToLevelData(string data)
        {
            levelOrderList = JsonConvert.DeserializeObject<List<string>>(data);
        }
        
    }
}