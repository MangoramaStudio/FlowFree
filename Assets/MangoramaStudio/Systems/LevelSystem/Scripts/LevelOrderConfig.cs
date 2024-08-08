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
            //ParseJsonToLevelData(levelOrder);
        }
        
        [RemoteSetting("levelOrder")]
        [SerializeField] private string levelOrder;
        [RemoteSetting("loopLevelOrder")]
        [SerializeField] private string loopLevelOrder;

        public string LevelOrder => levelOrder;
        public string LoopLevelOrder => loopLevelOrder;
        
        public List<int> DisplayLevelDataList => Convert(LevelOrder);
        
        public List<int> DisplayLoopLevelDataList => Convert(LoopLevelOrder);

        
        private List<int> Convert(string order)
        {
            List<int> list= new();
            var numberArray = order.Split(',');
            foreach (var number in numberArray)
            {
                if (int.TryParse(number, out var parsedNumber))
                {
                    list.Add(parsedNumber);
                }
                else
                {
                    Debug.LogError("Failed to parse number: " + number);
                }
            }

            return list;
        }

        
        
        /*
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
        */
        
    }
}