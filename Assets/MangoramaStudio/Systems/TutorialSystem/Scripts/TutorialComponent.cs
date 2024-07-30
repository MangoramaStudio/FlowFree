using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        public TutorialDefinition GetCurrentTutorialDefinition => TutorialDefinitions.ElementAt(CurrentTutorialIndex);

        private Sequence _tutorialSequence;

        private void StartHand()
        {
            tutorialHand.transform.SetParent(GetCurrentTutorialDefinition.flowDrawer.GetCorrectOrderedCells().First().transform);
            tutorialHand.transform.localPosition = Vector3.zero;

            _tutorialSequence?.Kill(true);
            _tutorialSequence = DOTween.Sequence();
            _tutorialSequence.Append(tutorialHand.GetComponentInChildren<SpriteRenderer>().DOFade(1f, .3f).From(0f));
            var list = GetCurrentTutorialDefinition.flowDrawer.GetCorrectOrderedCells();

            for (var index = 0; index < list.Count; index++)
            {
                var pos = list[index];
                var newPos = new Vector3(pos.transform.position.x, 1f, pos.transform.position.z);
                var index1 = index;

                _tutorialSequence.Append(tutorialHand.transform.DOMove(newPos, .35f).SetEase(Ease.Linear));
        
                if (index == list.Count - 1)
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
            if (CurrentTutorialIndex > TutorialDefinitions.Count)
            {
                CurrentTutorialIndex = TutorialDefinitions.Count;
            }
        }
        
        public int CurrentTutorialIndex { get; set; }
    }

    [Serializable]
    public class TutorialDefinition
    {
        public FlowDrawer flowDrawer;
        public bool isCompleted;
    }
}