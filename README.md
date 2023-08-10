# Auto Settings

## Overview

**Custom Runtime and Editor Settings in Unity**  
This Unity project provides a way to manage both project-wide and per-user settings within Unity.

With the provided `EditorSettingsProvider`, you can instantly access settings under either *Edit/Preferences* or *Edit/Project Settings*, depending on the setting type.

## Key Concepts

- **Editor User Settings**: Specific to individual users within the editor.
- **Editor Project Settings**: Applied across the entire project within the editor.
- **Runtime Project Settings**: Project-wide settings accessible at runtime but configurable in the editor.

## Example

Here's an example that demonstrates how to expose project-wide editor settings using the `AutoSetting` class and attribute.
This setting will 

```csharp
[AutoSetting(SettingUsage.EditorProject, "SettingName")]
public class ExampleSetting : AutoSetting<ExampleSetting>
{
	[SerializeField] private string _stringSetting;

	[SettingsProvider]
	public static SettingsProvider CreateSettingsProvider()
	{
		return EditorSettingsProvider<ExampleSetting>.Create();
	}
}
```

## Notes
- Setting storage strategy and folders are currently hard-coded, this will be addressed in a future update.
- The user settings folder should be excluded from source control.

## License and Credits
- This work is licensed under CC BY 4.0. To view a copy of this license, visit http://creativecommons.org/licenses/by/4.0/
- The setting implementation is inspired by the article [Custom Runtime and Editor Settings in Unity](https://HextantStudios.com/unity-custom-settings)
