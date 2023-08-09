using ShadyPixel.AutoSettings.Editor;
using UnityEditor;
using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    [AutoSetting(SettingUsage.EditorUser, "EditorUserSettingsExample", "Megapop")]
    public class EditorUserSettingsExample : AutoSetting<EditorUserSettingsExample>
    {
        [SerializeField] private string _testString;
        [SerializeField] private int _testInt;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return EditorSettingsProvider<EditorUserSettingsExample>.Create();
        }
    }
}