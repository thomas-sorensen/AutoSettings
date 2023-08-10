using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShadyPixel.AutoSettings.Editor
{
    /// <summary>
    ///     Provides a custom settings provider for the Unity editor, enabling the management and editing of specific setting types.
    ///     This class leverages UnityEditor.Editor.CreateEditor to support custom editors, allowing users to tailor the behavior of the settings panel.
    ///     Inspired by UnityEditor.AssetSettingsProvider.
    /// </summary>
    /// <typeparam name="TSetting">The specific type of setting that this provider handles. Must be a subclass of AutoSetting<TSetting>.</typeparam>
    public class EditorSettingsProvider<TSetting> : SettingsProvider
        where TSetting : AutoSetting<TSetting>
    {
        private const string EditorUserPathPrefix = "Preferences/";
        private const string EditorProjectPathPrefix = "Project/";

        private UnityEditor.Editor _editor;
        private bool _generatedKeywords;
        private TSetting _setting;

        public EditorSettingsProvider(string path, SettingsScope scopes)
            : base(path, scopes)
        {
        }

        /// <summary>
        ///     Retrieves the setting or logs an error if it fails.
        /// </summary>
        private static TSetting GetSetting()
        {
            var setting = AutoSetting<TSetting>.Get();
            if (setting) return setting;

            Debug.LogError($"{typeof(EditorSettingsProvider<TSetting>)}: Failed to retrieve setting");
            return null;
        }


        /// <summary>
        ///     Factory method to create an EditorSettingsProvider.
        /// </summary>
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
            RecreateEditorIfNeeded();
            base.OnActivate(searchContext, rootElement);
        }


        /// <summary>
        ///     Recreates the editor if it's not found.
        /// </summary>
        private void RecreateEditorIfNeeded()
        {
            if (_setting && _editor) return;

            LoadSetting();
            ReleaseEditor();
            CreateEditor();
        }

        /// <summary>
        ///     Loads the setting or logs an error if it fails.
        /// </summary>
        private void LoadSetting()
        {
            _setting = GetSetting();
            if (_setting) return;

            Debug.LogError($"{GetType()}: Failed to retrieve setting of type {typeof(TSetting)}");
        }

        /// <summary>
        ///     Creates an editor instance or logs an error if it fails.
        /// </summary>
        private void CreateEditor()
        {
            _editor = UnityEditor.Editor.CreateEditor(_setting);
            if (_editor) return;

            Debug.LogError($"{GetType()}: Failed to create editor!");
        }

        /// <summary>
        ///     Releases the editor instance if it exists.
        /// </summary>
        private void ReleaseEditor()
        {
            if (!_editor) return;

            Object.DestroyImmediate(_editor);
            _editor = null;
        }

        public override void OnDeactivate()
        {
            if (_editor)
            {
                //TODO: Might want to move this into ReleaseEditor (need to check it).
                if (_editor.serializedObject.targetObject != null)
                {
                    _editor.serializedObject.ApplyModifiedProperties();
                }

                Object.DestroyImmediate(_editor);
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
            RecreateEditorIfNeeded();

            if (!_editor)
            {
                EditorGUILayout.LabelField($"Failed to create an editor for {typeof(TSetting)}");
                base.OnGUI(searchContext);
                return;
            }

            EditorGUI.BeginChangeCheck();

            using (new SettingsGuiScope())
            {
                _editor.OnInspectorGUI();
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