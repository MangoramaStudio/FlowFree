using MangoramaStudio.Scripts.Data;
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.RoboticFlows
{
    public class LevelCounterUIController : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelCounterTMP;
        
        public void Initialize()
        {
            var data = GameManager.Instance.DataManager.GetData<LevelData>();
            levelCounterTMP.SetText($"Level {data.currentLevelIndex+1}");
        }
        
    }
}