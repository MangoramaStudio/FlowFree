using MangoramaStudio.Scripts.Data;
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
            levelCounterTMP.SetText($"Level {PlayerData.CurrentLevelId+1}");
        }
    }
}