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
        /// asset file destination path
        /// </summary>
        public string apkAssetPath;
    }
}