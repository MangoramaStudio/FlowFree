using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MangoramaStudio.Systems.PopupSystem.Scripts
{
    public enum PopupType
    {
        Default,
        Tutorial,
        Settings,
        AdsNotReady,
        CoverAllTiles,
    }
    public abstract class PopupBase : UIBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private GameObject content;
        public virtual void Show()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(content.transform.DOScale(1f, .15f).SetEase(Ease.OutBack).From(0f));
        }

        public virtual void Hide()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(content.transform.DOScale(0f, .15f).SetEase(Ease.InOutSine));
            sequence.AppendCallback(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}