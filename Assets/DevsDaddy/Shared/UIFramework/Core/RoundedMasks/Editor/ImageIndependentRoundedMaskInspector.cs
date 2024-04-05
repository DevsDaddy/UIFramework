using UnityEditor;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.RoundedMasks.Editor
{
    [CustomEditor(typeof(ImageIndependentRoundedMask))]
    public class ImageIndependentRoundedMaskInspector : UnityEditor.Editor
    {
        private ImageIndependentRoundedMask script;

        private void OnEnable() {
            script = (ImageIndependentRoundedMask)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (!script.TryGetComponent<MaskableGraphic>(out var _)) {
                EditorGUILayout.HelpBox("This script requires an MaskableGraphic (Image or RawImage) component on the same gameobject", MessageType.Warning);
            }
        }
    }
}