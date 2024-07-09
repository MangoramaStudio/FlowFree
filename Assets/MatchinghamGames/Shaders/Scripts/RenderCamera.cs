using UnityEngine;

namespace Shaders.Scripts
{
    public class RenderCamera : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private RenderTexture renderTexture;

        private void LateUpdate()
        {
            if (renderCamera == null || renderTexture == null) return;
            ActivateRenderTexture(renderTexture);
            var tex = CreateTexture2D();
            ActivateRenderTexture(null);
            CreateSprite(tex);
        }

        private Texture2D CreateTexture2D()
        {
            var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            return tex;
        }

        private void ActivateRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
        }

        private void CreateSprite(Texture2D tex)
        {
            spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));   
        }
    }
}