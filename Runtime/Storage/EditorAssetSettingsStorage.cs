using System.Text;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#else
using System;
#endif

namespace ShadyPixel.AutoSettings
{
    public class EditorAssetSettingsStorage : AssetSettingsStorage
    {
        public EditorAssetSettingsStorage(string folderPath)
            : base(folderPath)
        {
        }

#if UNITY_EDITOR
        private static IEnumerable<T> FindAssetsByType<T>() where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    yield return asset;
                }
            }
        }

#endif

        protected override TSetting LoadAsset<TSetting>(string path)
        {
#if UNITY_EDITOR
            var setting = AssetDatabase.LoadAssetAtPath<TSetting>(path);
            if (setting) return setting;

            var guids = AssetDatabase.FindAssets($"t:{typeof(TSetting)}");
            if (guids.Length <= 0) return null;

            var sourcePath = AssetDatabase.GUIDToAssetPath(guids[0]);

            var sb = new StringBuilder();
            sb.AppendLine($"Found {typeof(TSetting)} in the wrong folder!");
            sb.Append($"Trying to move the asset from {sourcePath} to {path}");
            Debug.LogWarning(sb.ToString());

            var result = AssetDatabase.MoveAsset(sourcePath, path);
            if (string.IsNullOrEmpty(result))
            {
                return AssetDatabase.LoadAssetAtPath<TSetting>(path);
            }

            Debug.LogWarning("Failed to move setting asset!");
            return null;

#else
            throw new InvalidOperationException($"Trying to use {nameof(EditorAssetSettingsStorage)} outside of the editor");
#endif
        }
    }
}