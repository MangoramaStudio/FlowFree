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
        
        [SerializeField] private List<string> loopLevelOrderList = new();
        
        //[RemoteSetting("levelOrder")]
        [SerializeField] private string levelOrder;

        [SerializeField] private string loopLevelOrder;

        public List<string> LevelOrder => JsonConvert.DeserializeObject<List<string>>(levelOrder);
        
        public List<string> LoopLevelOrder => JsonConvert.DeserializeObject<List<string>>(loopLevelOrder);

        
        [InfoBox("This fills the string with the level data struct you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large),GUIColor(0,1,1)]
        private void ConvertLevelDataToJson()
        {
            levelOrderList.Clear();
            levelOrder = JsonConvert.SerializeObject(levelOrderList);
        }
        
        [InfoBox("This fills the string with the loop level data struct you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large),GUIColor(0,1,1)]
        private void ConvertLoopLevelDataToJson()
        {
            loopLevelOrderList.Clear();
            loopLevelOrder = JsonConvert.SerializeObject(levelOrderList);
        }
        
        [InfoBox("This fills the level data struct list with the json string you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large)]
        public void ParseJsonToLevelData(string data)
        {
            levelOrderList.Clear();
            levelOrderList = JsonConvert.DeserializeObject<List<string>>(data);
        }
        
        [InfoBox("This fills the level data struct list with the json string you will set", InfoMessageType.Warning)]
        [Button(ButtonSizes.Large)]
        public void ParseJsonToLoopLevelData()
        {
            loopLevelOrderList.Clear();
            loopLevelOrderList = JsonConvert.DeserializeObject<List<string>>(loopLevelOrder);
        }
        
    }
}