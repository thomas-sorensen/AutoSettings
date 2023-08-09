#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#else
using System;
#endif


namespace ShadyPixel.AutoSettings
{
    public class EditorPrefSettingsStorage : ISettingStorage
    {
        public TSetting Load<TSetting>(SettingUsage settingUsage, string scope, string settingName)
            where TSetting : AutoSetting<TSetting>
        {
#if UNITY_EDITOR
            var key = AutoSetting<TSetting>.CreateKey(scope, settingName);
            var data = EditorPrefs.GetString(key);

            var setting = ScriptableObject.CreateInstance<TSetting>();
            if (string.IsNullOrEmpty(data)) return setting;

            // We can't deserialize the object directly, since it's a ScriptableObject
            EditorJsonUtility.FromJsonOverwrite(data, setting);

            return setting;
#else
            throw new InvalidOperationException($"Trying to load editor setting {typeof(TSetting)} runtime");
#endif
        }

        public void Save<TSetting>(TSetting setting) where TSetting : AutoSetting<TSetting>
        {
#if UNITY_EDITOR
            var key = setting.Key;
            var data = EditorJsonUtility.ToJson(setting);

            EditorPrefs.SetString(key, data);
#else
            throw new InvalidOperationException($"Trying to save editor setting {typeof(TSetting)} runtime");
#endif
        }
    }
}