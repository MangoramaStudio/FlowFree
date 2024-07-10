using System;
using DG.Tweening;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private float surfaceSize;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public int Id => id;

        public Color Color => spriteRenderer.color;
        
        public void SetId(int i)
        {
            id = i;
        }

        public void SetColor(Color c)
        {
            spriteRenderer.color = c;
        }
        
        public Tween Bounce()
        {
            var targetTransform = spriteRenderer.transform;
            
            targetTransform.DOKill();
            targetTransform.localScale = surfaceSize * Vector3.one;
            
            var tween = targetTransform.DOPunchScale(0.1f * Vector3.one, 0.5f, 1).SetEase(Ease.OutSine);
            return tween;
        }

        public void AnimateIn(float duration, float delay, Action onComplete = null)
        {
            spriteRenderer.DOKill();
            spriteRenderer.transform.DOKill();
            spriteRenderer.transform.localScale = Vector3.zero;
            
            var c = spriteRenderer.color;
            c.a = 0;
            spriteRenderer.color = c;

            spriteRenderer.transform.DOScale(surfaceSize, duration).SetEase(Ease.OutBack).SetDelay(delay);
            spriteRenderer.DOFade(1, duration).SetDelay(delay).SetEase(Ease.OutBack).OnComplete(() => onComplete?.Invoke());
        }
    }
}