
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class WinMenu : BaseMenu
    {
        [SerializeField] private TextMeshProUGUI areaTextTMP;

        public override void Initialize()
        {
            base.Initialize();
            SetText();
        }

        public void SetText()
        {
            var move = GameManager.Instance.UIManager.GameplayMenu().GetMoveCount();
            areaTextTMP.SetText($"You cleared in {move} moves");
        }
    }
}