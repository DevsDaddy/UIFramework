using DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Effects;
using UnityEditor;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Editor
{
    [CustomEditor(typeof(UIHsv))]
    [CanEditMultipleObjects]
    public class UIHsvModifierEditor : UnityEditor.Editor
    {
        SerializedProperty _spTargetColor;
        SerializedProperty _spRange;
        SerializedProperty _spHue;
        SerializedProperty _spSaturation;
        SerializedProperty _spValue;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected void OnEnable()
        {
            _spTargetColor = serializedObject.FindProperty("m_TargetColor");
            _spRange = serializedObject.FindProperty("m_Range");
            _spHue = serializedObject.FindProperty("m_Hue");
            _spSaturation = serializedObject.FindProperty("m_Saturation");
            _spValue = serializedObject.FindProperty("m_Value");
        }


        /// <summary>
        /// Implement this function to make a custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //================
            // Effect setting.
            //================
            EditorGUILayout.PropertyField(_spTargetColor);
            EditorGUILayout.PropertyField(_spRange);
            EditorGUILayout.PropertyField(_spHue);
            EditorGUILayout.PropertyField(_spSaturation);
            EditorGUILayout.PropertyField(_spValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}