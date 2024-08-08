using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Systems.TutorialSystem.Scripts
{
    public class TutorialComponent : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialHand;
        [SerializeField] private List<TutorialDefinition> tutorialDefinitions = new();
        public List<TutorialDefinition> TutorialDefinitions => tutorialDefinitions;
        public TutorialDefinition GetCurrentTutorialDefinition => AllCompleted() ? null : TutorialDefinitions.ElementAt(CurrentTutorialIndex);

        private Sequence _tutorialSequence;
        
        public bool IsCompleted { get; private set; }

        private void Start()
        {
            CurrentTutorialIndex = 0;
            tutorialDefinitions.ForEach(x=>x.mask.gameObject.SetActive(false));
            tutorialDefinitions.ElementAt(CurrentTutorialIndex).mask.gameObject.SetActive(true);
            
            GameManager.Instance.EventManager.PlayTutorial(CurrentTutorialDefinition().definition);
        }

        private TutorialDefinition CurrentTutorialDefinition()
        {
            return tutorialDefinitions.ElementAt(CurrentTutorialIndex);
        }

        private void StartHand()
        {

            var orderedCellsList = GetCurrentTutorialDefinition.flowDrawer.GetCorrectOrderedCells();
            if (CurrentTutorialDefinition().reverseDirection)
            {
                orderedCellsList.Reverse();
            }
            
            tutorialHand.transform.SetParent(orderedCellsList.First().transform);
            tutorialHand.transform.localPosition = Vector3.zero;

            _tutorialSequence?.Kill(true);
            _tutorialSequence = DOTween.Sequence();
            _tutorialSequence.Append(tutorialHand.GetComponentInChildren<SpriteRenderer>().DOFade(1f, .3f).From(0f));
           

            for (var index = 0; index < orderedCellsList.Count; index++)
            {
                var pos = orderedCellsList[index];
                var newPos = new Vector3(pos.transform.position.x, 1f, pos.transform.position.z);
                var index1 = index;

                _tutorialSequence.Append(tutorialHand.transform.DOMove(newPos, .35f).SetEase(Ease.Linear));
        
                if (index == orderedCellsList.Count - 1)
                {
                    _tutorialSequence.Append(tutorialHand.GetComponentInChildren<SpriteRenderer>().DOFade(0, 0.5f));
                    _tutorialSequence.AppendInterval(.5f);
                }
            }

            _tutorialSequence.OnComplete(() =>
            {
                _tutorialSequence.Restart();
            });

            _tutorialSequence.Play().SetLoops(-1, LoopType.Restart);
        }

        
        public void SetPlayableCells(List<Cell> cells)
        {
            if (AllCompleted())
            {
                Debug.LogError("all completed");
                tutorialHand.gameObject.SetActive(false);
                cells.ForEach(x => x.GetComponent<BoxCollider>().enabled = true);
                GameManager.Instance.EventManager.CompleteTutorial();
                IsCompleted = true;
                return;
            }
            cells.ForEach(x => x.GetComponent<BoxCollider>().enabled = false);
            GetCurrentTutorialDefinition.flowDrawer.GetCorrectOrderedCells().ForEach(x => x.GetComponent<BoxCollider>().enabled = true);
            StartHand();
        }

        private bool AllCompleted()
        {
            return TutorialDefinitions.All(x => x.isCompleted);
        }
        
        public void TryIncrementTutorialIndex()
        {
            CurrentTutorialIndex++;
            tutorialDefinitions.ForEach(x=>x.mask.gameObject.SetActive(false));
            if (CurrentTutorialIndex >= TutorialDefinitions.Count)
            {
                CurrentTutorialIndex = TutorialDefinitions.Count;
                return;
            }
            tutorialDefinitions.ElementAt(CurrentTutorialIndex).mask.gameObject.SetActive(true);
            GameManager.Instance.EventManager.PlayTutorial(CurrentTutorialDefinition().definition);
            
        }
        
        public int CurrentTutorialIndex { get; set; }
    }

    [Serializable]
    public class TutorialDefinition
    {
        public FlowDrawer flowDrawer;
        public bool isCompleted;
        public GameObject mask;
        public string definition;
        public bool reverseDirection;
    }
}