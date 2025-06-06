using System;

namespace YVR.Utilities.Editor.PackingProcessor
{
    [Serializable]
    public class PackingAssetInfo
    {
        /// <summary>
        /// asset file source path
        /// </summary>
        public string unityAssetPath;

        /// <summary>
        /// Usage for this asset, e.g., "Splash", "ImageTracking", etc.
        /// </summary>
        public string usage;

        /// <summary>
        /// asset file destination path
        /// </summary>
        [NonSerialized] public string apkAssetPath;
    }
}