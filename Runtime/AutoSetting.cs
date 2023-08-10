using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    /// <summary>
    /// Represents an automatically managed setting within the application.
    /// </summary>
    [JsonObject(MemberSerialization.Fields)]
    public class AutoSetting<TSetting> : ScriptableObject where TSetting : AutoSetting<TSetting>
    {
        #region Singleton Pattern

        // Lazy<TSetting> fails when we exit PlayMode
        //private static readonly Lazy<TSetting> _lazyInstance = new(CreateInstance);
        //private static TSetting Instance => _lazyInstance.Value;
        private static TSetting _instance;

        private static TSetting Instance
        {
            get
            {
                if (_instance) return _instance;
                _instance = CreateInstance();
                return _instance;
            }
        }

        #endregion

        public static event Action<TSetting> Changed;

        [JsonIgnore] private ISettingStorage _storage;
        [JsonIgnore] public string Key => CreateKey(Scope, SettingName);
        [JsonIgnore] public string SettingName { get; private set; }
        [JsonIgnore] public string Scope { get; private set; }
        [JsonIgnore] public SettingUsage SettingUsage { get; private set; }

        /// <summary>
        /// Generates a unique key based on the provided scope and setting name.
        /// </summary>
        public static string CreateKey(string scope, string settingName)
        {
            return string.IsNullOrWhiteSpace(scope) ? settingName : $"{scope}.{settingName}";
        }

        /// <summary>
        /// Creates and initializes a new instance of the setting, loading its values from storage.
        /// </summary>
        /// <returns>A new instance of the setting.</returns>
        /// <exception cref="MissingAttributeException">Thrown when the required AutoSettingAttribute is missing on the TSetting type.</exception>
        private static TSetting CreateInstance()
        {
            var type = typeof(TSetting);
            var attribute = type.GetCustomAttribute<AutoSettingAttribute>()
                            ?? throw new MissingAttributeException(typeof(AutoSettingAttribute));

            var settingName = attribute.Name ?? type.Name;
            var settingUsage = attribute.Usage;
            var settingScope = attribute.Scope;
            var storage = SettingStorage.Get(settingUsage);

            var setting = storage.Load<TSetting>(settingUsage, settingScope, settingName);
            setting._storage = storage;
            setting.SettingName = settingName;
            setting.SettingUsage = settingUsage;
            setting.Scope = settingScope;

            return setting;
        }

        /// <summary>
        /// Retrieves the setting instance, creating it if necessary.
        /// </summary>
        public static TSetting Get()
        {
            return Instance;
        }

        /// <summary>
        /// Saves the current setting, invoking the Changed event if successful.
        /// </summary>
        public void Save()
        {
            // "this" doesn't have the inherited type, so we're preventing a cast by using instance
            // This way we're also checking for duplicate instances of the setting type.
            var instance = Instance;
            if (instance != this)
            {
                throw new InvalidOperationException(
                    $"Detected two instances of {nameof(TSetting)}: this={this}, instance={instance}");
            }

            instance._storage.Save(instance);

            Changed?.Invoke(Instance);
        }
    }
}