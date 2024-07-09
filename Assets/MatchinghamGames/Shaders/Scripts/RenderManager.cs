using UnityEngine;

namespace Shaders.Scripts
{
    public class RenderManager : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private Transform renderTransform;
        [SerializeField] private RenderCamera renderCamera;
        [SerializeField] private SpriteRenderer renderSprite;
        [SerializeField] private GameObject targetObject;
        
        public Material RenderSpriteMaterial { get; private set; }

        private void Awake()
        {
            if (!isActive)
            {
                TryActivateRenderObjects(isActive);
                return;
            }
            InstantiateTargetObject();
            CloneRenderSpriteMaterial();
        }
        
        
        private void TryActivateRenderObjects(bool active)
        {
            renderCamera.gameObject.SetActive(active);
            renderSprite.gameObject.SetActive(active);
        }

        private void InstantiateTargetObject()
        {
            var obj = Instantiate(targetObject, renderTransform);
            obj.transform.localPosition = Vector3.zero;
            targetObject.gameObject.SetActive(false);
        }

        private void CloneRenderSpriteMaterial()
        {
            var component = renderSprite.GetComponent<Renderer>();
            RenderSpriteMaterial = new Material(component.material);
            component.material = RenderSpriteMaterial;
        }
    }
}