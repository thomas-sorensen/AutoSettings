using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;
using UEditor = UnityEditor.Editor;

namespace ShadyPixel.AutoSettings.Editor
{
    /// <summary>
    ///     Based on UnityEditor.AssetSettingsProvider
    /// </summary>
    public class EditorSettingsProvider<TSetting> : SettingsProvider
        where TSetting : AutoSetting<TSetting>
    {
        private const string EditorUserPathPrefix = "Preferences/";
        private const string EditorProjectPathPrefix = "Project/";

        private UEditor _editor;
        private bool _generatedKeywords;
        private TSetting _setting;

        public EditorSettingsProvider(string path, SettingsScope scopes)
            : base(path, scopes)
        {
        }

        private static TSetting GetSetting()
        {
            var setting = AutoSetting<TSetting>.Get();
            if (setting) return setting;

            Debug.LogError($"{typeof(EditorSettingsProvider<TSetting>)}: Failed to retrieve setting");
            return null;
        }

        public static EditorSettingsProvider<TSetting> Create()
        {
            var targetSetting = GetSetting();
            var isProjectSetting = targetSetting.SettingUsage.IsProjectSetting();

            var settingsScope = isProjectSetting ? SettingsScope.Project : SettingsScope.User;
            var basePath = isProjectSetting ? EditorProjectPathPrefix : EditorUserPathPrefix;

            var editorPath = basePath + targetSetting.Key.Replace('.', '/');

            var provider = new EditorSettingsProvider<TSetting>(editorPath, settingsScope)
            {
                label = ObjectNames.NicifyVariableName(targetSetting.SettingName),
                _setting = targetSetting
            };

            return provider;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (!_setting || !_editor)
            {
                RecreateEditor();
            }

            base.OnActivate(searchContext, rootElement);
        }

        private void RecreateEditor()
        {
            LoadSetting();
            ReleaseEditor();
            CreateEditor();
        }

        private void LoadSetting()
        {
            _setting = GetSetting();
            if (_setting) return;

            Debug.LogError($"{GetType()}: Failed to retrieve setting of type {typeof(TSetting)}");
        }

        private void CreateEditor()
        {
            _editor = UEditor.CreateEditor(_setting);
            if (_editor) return;

            Debug.LogError($"{GetType()}: Failed to create editor!");
        }

        private void ReleaseEditor()
        {
            if (!_editor) return;

            UObject.DestroyImmediate(_editor);
            _editor = null;
        }

        public override void OnDeactivate()
        {
            if (_editor)
            {
                if (_editor.serializedObject.targetObject != null)
                {
                    _editor.serializedObject.ApplyModifiedProperties();
                }

                UObject.DestroyImmediate(_editor);
                _editor = null;
            }

            if (_setting)
            {
                _setting.Save();
            }

            base.OnDeactivate();
        }

        public override void OnGUI(string searchContext)
        {
            if (!_setting || !_editor)
            {
                RecreateEditor();
            }

            if (!_editor)
            {
                EditorGUILayout.LabelField($"Failed to create an editor for {typeof(TSetting)}");
                base.OnGUI(searchContext);
                return;
            }

            EditorGUI.BeginChangeCheck();
            {
                using (new SettingsGuiScope())
                {
                    _editor.OnInspectorGUI();
                }
            }

            if (EditorGUI.EndChangeCheck() && _setting)
            {
                _setting.Save();
            }

            base.OnGUI(searchContext);
        }

        public override bool HasSearchInterest(string searchContext)
        {
            if (_generatedKeywords || !_editor)
            {
                return base.HasSearchInterest(searchContext);
            }

            keywords = GetSearchKeywordsFromSerializedObject(_editor.serializedObject);
            _generatedKeywords = true;
            return base.HasSearchInterest(searchContext);
        }
    }
}