using System;
using System.Diagnostics.CodeAnalysis;

namespace ShadyPixel.AutoSettings
{
    /// <summary>
    /// Attribute for defining metadata about an automatic setting.
    /// The 'Name' and 'Scope' properties are combined to create a unique key for the setting.
    /// </summary>
    public class AutoSettingAttribute : Attribute
    {
        /// <summary>
        /// Gets the context in which the setting is used.
        /// </summary>
        public SettingUsage Usage { get; }

        /// <summary>
        /// Gets the unique name of the setting.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the scope of the setting.
        /// </summary>
        public string Scope { get; }

        public AutoSettingAttribute(SettingUsage settingUsage)
        {
            Usage = settingUsage;
        }

        public AutoSettingAttribute(SettingUsage settingUsage, [NotNull] string name, string settingScope = null)
        {
            Usage = settingUsage;
            Name = name;
            Scope = settingScope;
        }
    }
}