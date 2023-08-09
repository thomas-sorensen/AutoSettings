using ShadyPixel.AutoSettings.Internal;
using UnityEditor;
using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    public abstract class AssetSettingsStorage : ISettingStorage
    {
        protected readonly string FolderPath;

        protected AssetSettingsStorage(string folderPath)
        {
            FolderPath = folderPath;
        }

        public TSetting Load<TSetting>(SettingUsage settingUsage, string scope, string settingName)
            where TSetting : AutoSetting<TSetting>
        {
            var path = CreatePath(scope, settingName);
            var instance = LoadAsset<TSetting>(path);

            if (instance != null) return instance;

#if UNITY_EDITOR
            instance = ScriptableObject.CreateInstance<TSetting>();
            PathUtil.CreateDirectory(path);
            AssetDatabase.CreateAsset(instance, path);
#endif
            return instance;
        }

        public void Save<TSetting>(TSetting setting) where TSetting : AutoSetting<TSetting>
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(setting);
#endif
        }

        protected abstract TSetting LoadAsset<TSetting>(string path) where TSetting : AutoSetting<TSetting>;

        private string CreatePath(string scope, string settingName)
        {
            scope = scope?.Replace('.', '/');

            var path = string.IsNullOrWhiteSpace(scope)
                ? FolderPath
                : $"{FolderPath}/{scope}";

            return PathUtil.Combine(path, settingName, ".asset");
        }
    }
}