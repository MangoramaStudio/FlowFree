using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Mechanics.RoboticFlows.Obstacles;
using Sirenix.OdinInspector;
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
        [SerializeField] private Sprite tileSprite;
        public Sprite TileSprite => tileSprite;
        
        private Sequence _blinkSequence;
        public Color Color => color;

        public Color OccupiedColor => occupiedColor;
        public bool IsBlinking => _blinkSequence?.IsPlaying() ?? false;
        public bool IsOccupied { get; private set; }
        [ShowInInspector]public bool HasObstacles => Obstacles.Count > 0;
        public List<Obstacle> Obstacles { get; private set; } = new();

        public void SetTileSprite(Sprite sprite)
        {
            tileSprite = sprite;
        }
        
        public void SetSpriteRenderer()
        {
            spriteRenderer.sprite = TileSprite;
            spriteRenderer.color = Color.white;
            spriteRenderer.DOFade(1f,0f);
            spriteRenderer.transform.localScale = Vector3.one * 0.3f;
        }
        
        
        private void Awake()
        {
            TryAddObstacles();
        }

        private void TryAddObstacles()
        {
            Obstacles.Clear();
            Obstacles = GetComponentsInChildren<Obstacle>().ToList(); 
        }

        public void SetOccupiedColor(Color c)
        {
            occupiedColor = c;
            if (IsOccupied)
            {
                //spriteRenderer.color = c;  
            }
        }

        public void SetColor(Color c)
        {
            color = c;
            if (!IsOccupied)
            {
                //spriteRenderer.color = c;
                spriteRenderer.color = Color.white;
            }
               
        }

        private Sequence _blobSeq;
        
        public void SetOccupied(bool occupiedState)
        {
            IsOccupied = occupiedState;
            //spriteRenderer.color = occupiedState ? occupiedColor : Color.white;
        }

        public void SetCompleteColor()
        {
            spriteRenderer.color = occupiedColor;
            spriteRenderer.DOFade(.5f, 0f);
        }
        
        public void PlayCompleteBlob(int id)
        {
            _blobSeq.Kill(true);
            _blobSeq = DOTween.Sequence();
            _blobSeq.AppendInterval(id * .05f);
            _blobSeq.Append(spriteRenderer.transform.DOScale(.33f, .15f).SetEase(Ease.InOutSine).OnStart(SetCompleteColor));
            _blobSeq.Append(spriteRenderer.transform.DOScale(.28f, .15f).SetEase(Ease.InOutSine));
            _blobSeq.Append(spriteRenderer.transform.DOScale(.3f, .15f).SetEase(Ease.InOutSine));
          
            
        }
        
        public void SetDefaultColor()
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.DOFade(.6f, 0f);
        }


        public void PlayBlob()
        {
            _blobSeq.Kill(true);
            _blobSeq = DOTween.Sequence();
            _blobSeq.Append(spriteRenderer.transform.DOScale(.33f, .15f).SetEase(Ease.InOutSine));
            _blobSeq.Append(spriteRenderer.transform.DOScale(.28f, .15f).SetEase(Ease.InOutSine));
            _blobSeq.Append(spriteRenderer.transform.DOScale(.3f, .15f).SetEase(Ease.InOutSine));
            
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
            SetDefaultColor();
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

        public void Blink(Color color)
        {
            if (IsOccupied)
                return;
            
            _blinkSequence?.Kill();
            _blinkSequence = DOTween.Sequence();
            _blinkSequence.Append(spriteRenderer.DOColor(color, 1f).SetEase(Ease.Linear));
            _blinkSequence.SetLoops(-1, LoopType.Restart);
        }

        public void StopBlink()
        {
            _blinkSequence?.Kill();
            spriteRenderer.DOKill();
            spriteRenderer.color = Color.white;
        }

        /// <summary>
        /// Find direction to target cell in order to handle obstacles and other features
        /// </summary>
        /// <param name="cell"></param>
        /// Target cell that will be the next target
        /// <returns></returns>
        public DirectionType DirectionAccordingToTargetCell(Cell cell)
        {
            var directionType = DirectionType.Left;
            
            if (x < cell.x && y <= cell.y )
            {
                directionType = DirectionType.Left;
            }
            else if (x > cell.x && y <= cell.y )
            {
                directionType = DirectionType.Right;
            }
            else if (x == cell.x && y < cell.y)
            {
                directionType = DirectionType.Down;
            }
            else if (x == cell.x && y > cell.y)
            {
                directionType = DirectionType.Up;
            }

            return directionType;

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