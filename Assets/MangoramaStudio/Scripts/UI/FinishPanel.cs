using MangoramaStudio.Scripts.Behaviours;
using MangoramaStudio.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace MangoramaStudio.Scripts.UI
{

    public class FinishPanel : UIPanel
    {

        public GameObject WinPanel;
        public GameObject LosePanel;

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);

           // UIManager.GameManager.EventManager.OnLevelFinished += SetWinLosePanel;
           // UIManager.GameManager.EventManager.OnLevelStarted += RefreshPanels;
        }


        private void OnDestroy()
        {
            //UIManager.GameManager.EventManager.OnLevelFinished -= SetWinLosePanel;
            //UIManager.GameManager.EventManager.OnLevelStarted -= RefreshPanels;
        }

        /****************************************************************************************/
        private void RefreshPanels()
        {
            WinPanel.SetActive(false);
            LosePanel.SetActive(false);
        }

        public override void ShowPanel()
        {
            base.ShowPanel();
        }

        private void SetWinLosePanel(bool isSuccess)
        {
            ShowPanel();

            StartCoroutine(SetWinLosePanelCo(isSuccess));
        }

        private IEnumerator SetWinLosePanelCo(bool isSuccess)
        {
            var delayTime = isSuccess == true ? FindObjectOfType<LevelBehaviour>().WinPanelDelayTime : FindObjectOfType<LevelBehaviour>().LosePanelDelayTime;

            yield return new WaitForSeconds(delayTime);

            WinPanel.SetActive(isSuccess);
            LosePanel.SetActive(!isSuccess);
        }
    }

}