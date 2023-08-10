
namespace ShadyPixel.AutoSettings
{
    public enum SettingUsage
    {
        RuntimeProject,
        EditorProject,
        EditorUser,
        //AppUser,
    }

    public static class SettingUsageExt
    {
        public static bool IsUserSetting(this SettingUsage usage)
        {
            return usage is SettingUsage.EditorUser;
            //return usage is SettingUsage.AppUser or SettingUsage.EditorUser;
        }

        public static bool IsProjectSetting(this SettingUsage usage)
        {
            return usage is SettingUsage.RuntimeProject or SettingUsage.EditorProject;
        }
    }
}
