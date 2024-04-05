using UnityEditor;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.RoundedMasks.Editor
{
    [CustomEditor(typeof(ImageRoundedMask))]
    public class ImageRoundedMaskInspector : UnityEditor.Editor
    { 
        private ImageRoundedMask script;

        private void OnEnable() {
            script = (ImageRoundedMask)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (!script.TryGetComponent<MaskableGraphic>(out var _)) {
                EditorGUILayout.HelpBox("This script requires an MaskableGraphic (Image or RawImage) component on the same gameobject", MessageType.Warning);
            }
        }
    }
}