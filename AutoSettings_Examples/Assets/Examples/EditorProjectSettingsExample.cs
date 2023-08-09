using ShadyPixel.AutoSettings.Editor;
using UnityEditor;
using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    [AutoSetting(SettingUsage.EditorProject, "EditorProjectSettingsExample", "Megapop")]
    public class EditorProjectSettingsExample : AutoSetting<EditorProjectSettingsExample>
    {
        [SerializeField] private string _testString;
        [SerializeField] private int _testInt;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return EditorSettingsProvider<EditorProjectSettingsExample>.Create();
        }
    }
}