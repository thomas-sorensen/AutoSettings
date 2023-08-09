using System.IO;
using UnityEngine;

namespace ShadyPixel.AutoSettings.Internal
{
    /// <summary>
    /// Provides utility methods for working with file paths, ensuring
    /// compatibility with Unity's directory structure and conventions.
    /// </summary>
    internal static class PathUtil
    {
        public const char UnityDirSeparator = '/';
        public const string AssetFolderName = "Assets";
        public const string AssetFolderPrefix = "Assets/";
        private const string EmptyString = "";

        /// Gets the file name and extension from a path string.
        public static string GetFileNameAndExtension(string path)
        {
            return Path.GetFileName(path);
        }

        /// Gets the file name without extension from a path string.
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// Gets the extension from a path string.
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// Replaces backslashes with Unity's directory separator.
        public static string UseUnityPathSeparator(string path)
        {
            return path.Replace('\\', UnityDirSeparator);
        }

        /// Replaces Unity's directory separator with the system's directory separator.
        public static string UseSystemPathSeparator(string path)
        {
            return UnityDirSeparator == Path.DirectorySeparatorChar
                ? path
                : path.Replace(UnityDirSeparator, Path.DirectorySeparatorChar);
        }

        /// Gets the directory name from a path string and ensures Unity directory separator.
        public static string GetDirectoryName(string path)
        {
            var dir = Path.GetDirectoryName(path);
            return string.IsNullOrWhiteSpace(dir)
                ? EmptyString
                : DirWithSeparator(UseUnityPathSeparator(dir));
        }

        /// Removes the "Assets/" prefix from a path string.
        public static string RemoveAssetFolderPrefix(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return EmptyString;
            if (path is AssetFolderPrefix or AssetFolderName) return EmptyString;

            return path.StartsWith(AssetFolderPrefix)
                ? path[AssetFolderPrefix.Length..]
                : path;
        }

        /// Converts a relative path to its full system path.
        public static string GetSystemPath(string path)
        {
            var systemPath = UseSystemPathSeparator(path);
            return Path.IsPathFullyQualified(systemPath)
                ? systemPath
                : UseSystemPathSeparator($"{Application.dataPath}{UnityDirSeparator}{RemoveAssetFolderPrefix(path)}");
        }

        /// Converts a full system path to its relative asset path.
        public static string GetRelativeAssetPath(string systemPath)
        {
            // Make sure we have a valid system path
            var path = GetSystemPath(systemPath);

            path = UseUnityPathSeparator(path);
            path = path.Replace(Application.dataPath, AssetFolderName);

            return path;
        }

        /// Creates a directory at the specified path.
        public static void CreateDirectory(string path)
        {
            var systemPath = GetSystemPath(path);
            var directory = Path.GetDirectoryName(systemPath);
            if (string.IsNullOrEmpty(directory)) return;

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        /// Prefixes the path with "Assets/". Assumes directory separator is '/'.
        public static string PrefixAssetFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return AssetFolderPrefix;

            path = UseUnityPathSeparator(path);
            if (path.StartsWith(AssetFolderPrefix)) return path;

            if (path == AssetFolderName) return AssetFolderPrefix;

            return (path.StartsWith(UnityDirSeparator))
                ? $"{AssetFolderName}{path}"
                : $"{AssetFolderPrefix}{path}";
        }

        /// Changes the file extension of a path.
        public static string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }

        /// Changes the name of the file in a path.
        public static string ChangeName(string path, string newName)
        {
            if (string.IsNullOrWhiteSpace(path)) return newName;

            var oldName = GetFileNameAndExtension(path);
            var oldDir = path[.. ^oldName.Length];
            var oldExt = GetExtension(oldName);

            return $"{oldDir}{newName}{oldExt}";
        }

        /// Changes both the name and extension of a file in a path.
        public static string ChangeNameAndExtension(string path, string newName, string newExt)
        {
            if (string.IsNullOrWhiteSpace(path)) return newName;

            var oldName = GetFileNameAndExtension(path);
            var oldDir = path[.. ^oldName.Length];
            var dotExt = DotExtension(newExt);

            return $"{oldDir}{newName}{dotExt}";
        }

        /// Adds a suffix to the file name in a path.
        public static string AddSuffix(string path, string suffix)
        {
            var oldName = GetFileNameWithoutExtension(path);
            return ChangeName(path, $"{oldName}{suffix}");
        }

        /// Adds a prefix to the file name in a path.
        public static string AddPrefix(string path, string prefix)
        {
            var oldName = GetFileNameWithoutExtension(path);
            return ChangeName(path, $"{prefix}{oldName}");
        }

        /// Adds a subfolder to a path.
        public static string AddSubfolderToPath(string path, string subfolder)
        {
            if (string.IsNullOrWhiteSpace(path)) return subfolder;

            var oldName = GetFileNameAndExtension(path);
            var oldDir = path[.. ^oldName.Length];

            return $"{oldDir}{subfolder}{UnityDirSeparator}{oldName}";
        }

        /// Prefixes a dot to the extension if one is missing.
        private static string DotExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext)) return EmptyString;
            return ext[0] == '.' ? ext : $".{ext}";
        }

        /// Ensures the directory ends with the Unity directory separator.
        private static string DirWithSeparator(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return "";
            return dir.EndsWith(UnityDirSeparator)
                ? dir
                : $"{dir}{UnityDirSeparator}";
        }

        /// Combines a directory and file name.
        public static string Combine(string dir, string name)
        {
            dir = DirWithSeparator(dir);
            return $"{dir}{name}";
        }

        /// Combines a directory, file name, and extension.
        public static string Combine(string dir, string name, string ext)
        {
            dir = DirWithSeparator(dir);
            ext = DotExtension(ext);
            return $"{dir}{name}{ext}";
        }
    }
}