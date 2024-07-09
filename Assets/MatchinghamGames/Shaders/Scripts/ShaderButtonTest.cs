using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Shaders.Scripts
{
    public class ShaderButtonTest : MonoBehaviour
    {
        [SerializeField] private ShaderManager shaderManager;
        [SerializeField] private Image image;
        private Material Material => image.material;

        [Button]
        public void EnableGreyScale()
        {
            ShaderExtensions.InitializeMaterial(image, ShaderExtensions.GreyScaleKeyword);
        }
        
        [Button]
        public void DisableGreyScale()
        {
            ShaderExtensions.DisposeMaterial(Material,ShaderExtensions.GreyScaleKeyword);
        }

        [Button]
        public void EnableWave()
        {
            ShaderExtensions.InitializeMaterial(image,ShaderExtensions.WaveKeyword);
        }

        [Button]
        public void DisableWave()
        {
            ShaderExtensions.DisposeMaterial(Material,ShaderExtensions.WaveKeyword);
        }
        
        private Sequence _shineSeq;
        [Button]
        public void UseShine()
        {
            ShaderExtensions.InitializeMaterial(image,ShaderExtensions.ShineKeyword,
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
            _shineSeq.Append(targetMaterial.DOFloat(end??1f, shaderID, duration??2f));
            _shineSeq.SetEase(ease??Ease.Linear);
            _shineSeq.SetDelay(delayStart??0f);
            _shineSeq.SetLoops(loopCount??-1, loopType?? LoopType.Restart); 
        }
    }
}