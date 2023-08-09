using UnityEditor;
using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    public class SettingsGuiScope : GUI.Scope
    {
        private readonly float _labelWidth;

        public SettingsGuiScope()
        {
            _labelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = 250;
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);
        }

        protected override void CloseScope()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = _labelWidth;
        }
    }
}
