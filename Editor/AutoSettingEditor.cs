using UnityEditor;

namespace ShadyPixel.AutoSettings
{
    [CustomEditor(typeof(AutoSetting<>), true )]
    public class AutoSettingEditor : UnityEditor.Editor
    {
        private const string ScriptPropertyName = "m_Script";

        /// <summary>
        /// Draw the default inspector, excluding the script property.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (serializedObject.targetObject == null) return;

            serializedObject.UpdateIfRequiredOrScript();
            DrawPropertiesExcluding(serializedObject, ScriptPropertyName);
            serializedObject.ApplyModifiedProperties();
        }
    }
}