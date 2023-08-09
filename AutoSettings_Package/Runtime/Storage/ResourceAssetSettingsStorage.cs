using UnityEngine;

namespace ShadyPixel.AutoSettings
{
    public class ResourceAssetSettingsStorage : AssetSettingsStorage
    {
        public ResourceAssetSettingsStorage(string folderPath)
            : base(folderPath)
        {
        }

        protected override TSetting LoadAsset<TSetting>(string path)
        {
            return Resources.Load<TSetting>(path);
        }
    }
}
