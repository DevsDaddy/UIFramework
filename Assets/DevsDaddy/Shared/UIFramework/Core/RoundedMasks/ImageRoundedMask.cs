using DevsDaddy.Shared.UIFramework.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.RoundedMasks
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UIFramework/Masks/Image Rounded Mask")]
    public class ImageRoundedMask : MonoBehaviour
    {
        private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");
        
        public float radius = 40f;          
        private Material material;
        
        [HideInInspector, SerializeField] private MaskableGraphic image;
        
        private void OnValidate() {
            Validate();
            Refresh();
        }
        
        private void OnDestroy() {
            image.material = null;
            DestroyHelper.Destroy(material);
            image = null;
            material = null;
        }

        private void OnEnable() {
            var other = GetComponent<ImageIndependentRoundedMask>();
            if (other != null) {
                radius = other.r.x; //When it does, transfer the radius value to this script
                DestroyHelper.Destroy(other);
            }
            
            Validate();
            Refresh();
        }
        
        private void OnRectTransformDimensionsChange() {
            if (enabled && material != null) {
                Refresh();
            }
        }

        public void Validate() {
            var isDirty = false;
            if (material == null) {
                material = new Material(Shader.Find("UIFramework/UI/RoundedCorners/RoundedCorners"));
                isDirty = true;
            }

            if (image == null) {
                TryGetComponent(out image);
                isDirty = true;
            }

            if (image != null) {
                image.material = material;
                isDirty = true;
            }
            
#if UNITY_EDITOR
            if (isDirty)
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
        
        public void Refresh() {
            var rect = ((RectTransform)transform).rect;
            material.SetVector(Props, new Vector4(rect.width, rect.height, radius * 2, 0));   
        }
    }
}