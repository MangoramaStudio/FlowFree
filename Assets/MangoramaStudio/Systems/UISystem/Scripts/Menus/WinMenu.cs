
using MangoramaStudio.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace MangoramaStudio.Systems.UISystem.Scripts.Menus
{
    public class WinMenu : BaseMenu
    {
        [SerializeField] private TextMeshProUGUI areaTextTMP;
        [SerializeField] private string preExplanation,postExplanation;
        [SerializeField] private Color color;
        public override void Initialize()
        {
            base.Initialize();
            SetText();
        }

        private void SetText()
        {
            var move = GameManager.Instance.UIManager.GameplayMenu().GetMoveCount();
          //  areaTextTMP.SetText($"You cleared in {move} moves");
          Set();
        }
        
        public string ColorString(string text, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        }

        public void Set()
        {
            var move = GameManager.Instance.UIManager.GameplayMenu().GetMoveCount();
            var coloredExplanation = ColorString(move.ToString(), color);
            var text = $"{preExplanation} {coloredExplanation} {postExplanation}";
            areaTextTMP.SetText(text);
        }
    }
}