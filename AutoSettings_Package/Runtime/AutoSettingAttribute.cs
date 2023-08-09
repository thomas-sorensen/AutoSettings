using System;
using System.Diagnostics.CodeAnalysis;

namespace ShadyPixel.AutoSettings
{
    public class AutoSettingAttribute : Attribute
    {
        public AutoSettingAttribute(SettingUsage settingUsage)
        {
            Usage = settingUsage;
        }

        public AutoSettingAttribute(SettingUsage settingUsage, [NotNull] string name, string settingScope = null,
            string editorPath = null)
        {
            Usage = settingUsage;
            Name = name;
            Scope = settingScope;
        }

        public SettingUsage Usage { get; }
        public string Name { get; }

        /// <summary>
        ///     Combined with the setting name to create a unique identifier to create a key/path
        ///     Each element is separated by a period '.'
        /// </summary>
        public string Scope { get; }
    }
}