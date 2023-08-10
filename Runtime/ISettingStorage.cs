namespace ShadyPixel.AutoSettings
{
    /// <summary>
    /// Defines the contract for managing the storage of settings within the application.
    /// </summary>
    public interface ISettingStorage
    {
        /// <summary>
        /// Loads a setting based on the provided context, scope, and name.
        /// </summary>
        /// <typeparam name="TSetting">Type of the setting.</typeparam>
        /// <param name="settingUsage">Usage context.</param>
        /// <param name="scope">Scope of the setting.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The loaded setting, or null if not found.</returns>
        public TSetting Load<TSetting>(SettingUsage settingUsage, string scope, string settingName)
            where TSetting : AutoSetting<TSetting>;

        /// <summary>
        /// Saves a setting to the appropriate storage location.
        /// </summary>
        /// <typeparam name="TSetting">Type of the setting.</typeparam>
        /// <param name="setting">Setting to be saved.</param>
        public void Save<TSetting>(TSetting setting) where TSetting : AutoSetting<TSetting>;
    }
}