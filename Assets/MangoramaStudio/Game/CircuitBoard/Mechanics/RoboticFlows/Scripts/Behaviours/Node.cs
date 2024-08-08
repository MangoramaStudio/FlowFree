using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private float surfaceSize;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite ballSprite;

        private RoboticFlowDrawer FlowDrawer => _flowDrawer ? _flowDrawer : (_flowDrawer = GetComponentInParent<RoboticFlowDrawer>());
        private RoboticFlowDrawer _flowDrawer;
        
        public int Id => id;

     

        public Sprite BallSprite => ballSprite;
        public Color Color => spriteRenderer.color;

        public void SetBallSprite(Sprite sprite)
        {
            ballSprite = sprite;
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = BallSprite;
            spriteRenderer.transform.localScale = Vector3.one * .25f;
        }
        
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
        
        public Tween ScaleUp()
        {
            var targetTransform = spriteRenderer.transform;
            
            targetTransform.DOKill();
            //targetTransform.localScale = surfaceSize * Vector3.one;
            var tween = targetTransform.DOPunchScale(0.05f * Vector3.one, 0.5f, 1).SetEase(Ease.OutSine).SetLoops(-1,LoopType.Yoyo);
            return tween;
        }
        
        public Tween ScaleDown()
        {
            var targetTransform = spriteRenderer.transform;
            
            targetTransform.DOKill();
            targetTransform.localScale = surfaceSize * Vector3.one;
            
            var tween = targetTransform.DOScale(surfaceSize,.1f).SetEase(Ease.InOutSine);
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