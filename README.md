# Auto Settings

## Overview

**Custom Runtime and Editor Settings in Unity**  
This Unity project provides a way to manage both project-wide and per-user settings within Unity.

With the provided `EditorSettingsProvider`, you can instantly access settings under either *Edit/Preferences* or *Edit/Project Settings*, depending on the setting type.

## Installation

To install the Auto Settings package in Unity, follow these steps:

1. Open your Unity project.
2. Navigate to **Window > Package Manager**.
3. Click the **+** button and select **Add package from git URL...**.
4. Enter the following URL: `https://github.com/thomas-sorensen/AutoSettings.git`.
5. Click **Add**, and Unity will handle the rest.

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
- The user settings folder should be excluded from source control.
- Setting storage strategy and folders are currently hard-coded; this will be addressed in a future update:
  - **Editor User Settings**: Stored using Unity's EditorPrefs, allowing for per-user preferences within the Unity editor.
  - **Editor Project Settings**: Stored as assets within the project, specifically located at the "Assets/Settings/Editor/Project" path.
  - **Runtime Project Settings**: These settings are stored as resources within the project at the "Assets/Settings/Resources" path.

## License and Credits
- This work is licensed under CC BY 4.0. To view a copy of this license, visit http://creativecommons.org/licenses/by/4.0/
- The setting implementation is inspired by the article [Custom Runtime and Editor Settings in Unity](https://HextantStudios.com/unity-custom-settings)
