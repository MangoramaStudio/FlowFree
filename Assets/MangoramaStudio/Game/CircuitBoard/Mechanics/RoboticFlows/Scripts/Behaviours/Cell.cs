using System;
using DG.Tweening;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class Cell : MonoBehaviour
    {
        public int x;
        public int y;
        public Node node;
        
        [SerializeField] private float surfaceSize;
        [SerializeField] private Color color;
        [SerializeField] private Color occupiedColor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject fill;

        private Sequence _blinkSequence;
        
        public Color Color => color;
        
        public Color OccupiedColor => occupiedColor;

        public bool IsBlinking => _blinkSequence?.IsPlaying() ?? false;
        
        public bool IsOccupied { get; private set; }

        public void SetOccupiedColor(Color c)
        {
            occupiedColor = c;
            if (IsOccupied)
                spriteRenderer.color = c;
        }

        public void SetColor(Color c)
        {
            color = c;
            if (!IsOccupied)
                spriteRenderer.color = c;
        }

        public void SetOccupied(bool occupiedState)
        {
            IsOccupied = occupiedState;
            spriteRenderer.color = occupiedState ? occupiedColor : color;

            
        }

        public void ShowFillHint(bool state)
        {
            if (state)
            {
                fill.gameObject.SetActive(true);
                FillHintAnimation();
            }
            else
            {
                fill.transform.DOKill();
                fill.gameObject.SetActive(false);
            }
        }

        public bool IsNeighbor(Cell cell)
        {
            return (cell.x == x && Mathf.Abs(cell.y - y) == 1) ||
                   (cell.y == y && Mathf.Abs(cell.x - x) == 1);
        }

        public void AnimateIn(float duration, float delay, Action onComplete = null)
        {
            spriteRenderer.DOKill();
            spriteRenderer.transform.DOKill();
            spriteRenderer.transform.localScale = Vector3.zero;

            var c = spriteRenderer.color;
            c.a = 0;
            spriteRenderer.color = c;

            if (node)
            {
                node.AnimateIn(duration, delay);
            }

            spriteRenderer.transform.DOScale(surfaceSize, duration).SetEase(Ease.OutBack).SetDelay(delay);
            spriteRenderer.DOFade(IsOccupied ? occupiedColor.a : color.a, duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void Blink()
        {
            if (IsOccupied)
                return;
            
            _blinkSequence?.Kill();
            _blinkSequence = DOTween.Sequence();
            _blinkSequence.Append(spriteRenderer.DOColor(occupiedColor, 0.5f).SetEase(Ease.Linear));
            _blinkSequence.Append(spriteRenderer.DOColor(color, 0.5f).SetEase(Ease.Linear));
            _blinkSequence.SetLoops(-1, LoopType.Restart);
        }

        public void StopBlink()
        {
            _blinkSequence?.Kill();
            spriteRenderer.DOKill();
            spriteRenderer.color = color;
        }

        private void FillHintAnimation()
        {
            fill.transform.DOKill();
            fill.transform.localScale = Vector3.one;

            Animation();
            

            void Animation()
            {
                fill.transform.DOScale(1.05f, 0.25f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    fill.transform.DOScale(1f, 0.25f).SetEase(Ease.InOutSine).OnComplete(Animation);
                });
            }
        }
    }
}