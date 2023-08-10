using System;

namespace ShadyPixel.AutoSettings
{
    /// <summary>
    /// Manages the storage of settings based on their usage context.
    /// TODO: Consider refactoring to allow selection of custom storage strategies through a separate factory pattern or dependency injection
    /// </summary>
    public class SettingStorage
    {
        /// Path to where runtime project settings will be stored.
        private const string RuntimeProjectStoragePath = "Assets/Settings/Resources";

        /// Path to where editor project settings will be stored.
        private const string EditorProjectStoragePath = "Assets/Settings/Editor/Project";

        /// Singleton instance of SettingStorage, lazily initialized.
        private static readonly Lazy<SettingStorage> _lazyInstance = new(CreateInstance);

        // Storage strategies for different usages.
        private ISettingStorage _editorProjectStorage;
        private ISettingStorage _editorUserStorage;
        private ISettingStorage _runtimeProjectStorage;

        /// Provides access to the singleton instance.
        private static SettingStorage Instance => _lazyInstance.Value;

        /// Factory method to create the singleton instance.
        private static SettingStorage CreateInstance()
        {
            return new SettingStorage
            {
                _runtimeProjectStorage = new ResourceAssetSettingsStorage(RuntimeProjectStoragePath),
                _editorProjectStorage = new EditorAssetSettingsStorage(EditorProjectStoragePath),
                _editorUserStorage = new EditorPrefSettingsStorage()
            };
        }

        /// <summary>
        /// Retrieves the appropriate storage strategy based on the setting usage.
        /// </summary>
        /// <param name="usage">The setting usage context.</param>
        /// <returns>The storage strategy for the given usage.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided usage does not match any known strategies.</exception>
        public static ISettingStorage Get(SettingUsage usage)
        {
            return usage switch
            {
                SettingUsage.RuntimeProject => Instance._runtimeProjectStorage,
                SettingUsage.EditorProject => Instance._editorProjectStorage,
                SettingUsage.EditorUser => Instance._editorUserStorage,
                _ => throw new ArgumentOutOfRangeException(nameof(usage), usage, null)
            };
        }
    }
}