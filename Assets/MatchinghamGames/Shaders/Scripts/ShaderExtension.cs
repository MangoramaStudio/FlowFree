using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Shaders.Scripts
{ 
    /// <summary>
    /// Includes for now
    /// - Shine - 24
    /// - Hue Shift - 8 
    /// - Clip UV  - 35
    /// - Fade - 2
    /// - Blur - 16
    /// - GreyScale -14
    /// </summary>
    public abstract class ShaderExtensions
    {
        
        #region Properties
        public static readonly int ShineLocation = Shader.PropertyToID(ShineLocationID);
        public static readonly int HueShiftRange = Shader.PropertyToID(HueShiftRangeID);
        public static readonly int ClipRight = Shader.PropertyToID(ClipUvRightID);
        public static readonly int ClipLeft = Shader.PropertyToID(ClipUvLeftID);
        public static readonly int ClipUp = Shader.PropertyToID(ClipUvUpID);
        public static readonly int ClipDown = Shader.PropertyToID(ClipUvDownID);
        public static readonly int FadeAmount = Shader.PropertyToID(FadeAmountID);
        #endregion

        #region Keywords
        public const string GreyScaleKeyword = "GREYSCALE_ON";
        public const string BlurKeyword = "BLUR_ON";
        public const string ShineKeyword ="SHINE_ON";
        public const string HueShiftKeyword = "HSV_ON";
        public const string ClippingKeyword = "CLIPPING_ON";
        public const string FadeKeyword = "FADE_ON";
        public const string WaveKeyword = "WAVEUV_ON";
        #endregion

        #region Parameters
        private const string ShineLocationID = "_ShineLocation";
        private const string HueShiftRangeID = "_HsvShift";
        private const string ClipUvRightID = "_ClipUvRight";
        private const string ClipUvLeftID = "_ClipUvLeft";
        private const string ClipUvUpID = "_ClipUvUp";
        private const string ClipUvDownID = "_ClipUvDown";
        private const string FadeAmountID = "_FadeAmount";
        #endregion
        

        /// <summary>
        /// Initializes the selected material
        /// </summary>
        /// <param name="targetObject">Material to be modified</param>
        /// <param name="keyword"> Keyword to be enabled or disabled </param>
        /// <param name="callback"> Callback for handling outer functions or parameters </param>
        
        public static void InitializeMaterial(Image targetObject,string keyword = null,Action<Material> callback = null)
        {
            var mat = !IsMaterialCloned(targetObject.material) 
                ? Object.Instantiate(targetObject.material) 
                : targetObject.material;
            targetObject.material = mat;
            
            if (!string.IsNullOrEmpty(keyword))
            { 
                EnableKeyword(mat,keyword);    
            }
            callback?.Invoke(mat);
        }
        
        public static void InitializeMaterial(Renderer targetObject,string keyword = null,Action<Material> callback = null)
        {
            var mat = !IsMaterialCloned(targetObject.material) 
                ? Object.Instantiate(targetObject.material) 
                : targetObject.material;
            targetObject.material = mat;
            
            if (!string.IsNullOrEmpty(keyword))
            { 
                EnableKeyword(mat,keyword);    
            }
            callback?.Invoke(mat);
        }

        /// <summary>
        /// Resets the selected material
        /// </summary>
        /// <param name="targetMaterial">Material to be modified</param>
        /// <param name="keyword"> Keyword to be enabled or disabled </param>
        /// <param name="callback"> Callback for handling outer functions or parameters </param>
        public static void DisposeMaterial(Material targetMaterial,string keyword=null,Action<Material> callback = null)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                DisableKeyword(targetMaterial,keyword); 
            }
            callback?.Invoke(targetMaterial);   
        }
        /// <summary>
        /// Enables the shader in selected material (not for global enabling)
        /// </summary>
        /// <param name="targetMaterial">Material to be modified</param>
        /// <param name="keyword"> Keyword to be enabled or disabled </param>
        private static void EnableKeyword(Material targetMaterial,string keyword)
        {
            targetMaterial.EnableKeyword(keyword);   
        }
        /// <summary>
        /// Disables the shader in selected material (not for global enabling)
        /// </summary>
        /// <param name="targetMaterial">Material to be modified</param>
        /// <param name="keyword"> Keyword to be enabled or disabled </param>
        private static void DisableKeyword(Material targetMaterial,string keyword)
        {
            targetMaterial.DisableKeyword(keyword);   
        }
        
        /// <summary>
        /// Checks material is cloned or not
        /// </summary>
        /// <param name="material">Target material to check</param>
        /// <returns></returns>
        private static bool IsMaterialCloned(Material material)
        {
            return material.name.Contains("(Clone)") || material.name.Contains("(Instance)");
        }
    }
}
