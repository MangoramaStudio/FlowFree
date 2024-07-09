using MangoramaStudio.Scripts.Managers;
using UnityEngine;

namespace MangoramaStudio.Scripts.UI
{
    public class UIPanel : MonoBehaviour
    {
        public UIManager UIManager { get; set; }

        public virtual void Initialize(UIManager uiManager)
        {
            UIManager = uiManager;
        }

        public virtual void PopulateView()
        {

        }

        public virtual void ShowPanel()
        {
            gameObject.SetActive(true);
        }

        public virtual void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}