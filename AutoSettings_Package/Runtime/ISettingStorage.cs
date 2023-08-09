namespace ShadyPixel.AutoSettings
{
    public interface ISettingStorage
    {
        public TSetting Load<TSetting>(SettingUsage settingUsage, string scope, string settingName)
            where TSetting : AutoSetting<TSetting>;

        public void Save<TSetting>(TSetting setting) where TSetting : AutoSetting<TSetting>;
    }
}