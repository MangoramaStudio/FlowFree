using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shaders.Scripts
{
    public class ShaderTest : MonoBehaviour
    {
        [SerializeField] private ShaderManager shaderManager;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Material Material => spriteRenderer.material;

        
        [Button]
        public void ClipLeft(bool left)
        {
            ShaderExtensions.InitializeMaterial(spriteRenderer,ShaderExtensions.ClippingKeyword,
                (x) =>
                {
                    var endValue = left ? 1f : 0f;
                    x.DOFloat(endValue, ShaderExtensions.ClipLeft, 3f);
                });
        }
        
        [Button]
        public void EnableGreyScale()
        {
            ShaderExtensions.InitializeMaterial(spriteRenderer,ShaderExtensions.GreyScaleKeyword);
        }
        
        [Button]
        public void DisableGreyScale()
        {
            ShaderExtensions.DisposeMaterial(Material,ShaderExtensions.GreyScaleKeyword);
        }
        
        [Button]
        public void EnableBlur()
        {
            ShaderExtensions.InitializeMaterial(spriteRenderer,ShaderExtensions.BlurKeyword);
        }
        
        [Button]
        public void DisableBlur()
        {
            ShaderExtensions.DisposeMaterial(Material,ShaderExtensions.BlurKeyword);
        }
        

        private Sequence _shineSeq;
        [Button]
        public void UseShine()
        {
            ShaderExtensions.InitializeMaterial(spriteRenderer,ShaderExtensions.ShineKeyword,
                (x) =>
                {
                    UseDefaultFloatSequence(x,ShaderExtensions.ShineLocation);
                });
        }

        [Button]
        public void DisableShine()
        {
            ShaderExtensions.DisposeMaterial(Material,ShaderExtensions.ShineKeyword,(x) =>
            {
                _shineSeq?.Kill(true);
                x.SetFloat(ShaderExtensions.ShineLocation,0f);
            });
        }

        [Button]
        public void EnableFade()
        {
            ShaderExtensions.InitializeMaterial(spriteRenderer, ShaderExtensions.FadeKeyword,
                (x) =>
                {
                    x.DOFloat(1f, ShaderExtensions.FadeAmount, 2f);
                });
        }
        
        [Button]
        public void DisableFade()
        {
            ShaderExtensions.DisposeMaterial(Material, ShaderExtensions.FadeKeyword);
        }
        
        private void UseDefaultFloatSequence(
            Material targetMaterial,
            int shaderID,
            float?delayStart =0f,
            float?start = 0f,
            float?end = 1f,
            float?duration = 1f,
            Ease? ease = Ease.Linear,
            int? loopCount = -1,
            LoopType? loopType = LoopType.Restart)
        {
            targetMaterial.SetFloat(shaderID, start??0f);
            _shineSeq?.Kill();
            _shineSeq = DOTween.Sequence();
            _shineSeq.Append(targetMaterial.DOFloat(end??1f, shaderID, duration??1f));
            _shineSeq.SetEase(ease??Ease.Linear);
            _shineSeq.SetDelay(delayStart??0f);
            _shineSeq.SetLoops(loopCount??-1, loopType?? LoopType.Restart); 
        }
    }
}