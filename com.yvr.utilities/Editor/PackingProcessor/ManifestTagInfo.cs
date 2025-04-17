namespace YVR.Utilities.Editor.PackingProcessor
{
    /// <summary>
    /// Represents information about a manifest tag, including its node path, tag name, attribute name, attribute value, and any additional attributes.
    /// </summary>
    [System.Serializable]
    public class ManifestTagInfo
    {
        /// <summary>
        /// its node path
        /// </summary>
        public string nodePath;

        /// <summary>
        /// The name of the manifest tag.
        /// </summary>
        public string tag;

        /// <summary>
        /// The name of the attribute for the manifest tag.
        /// </summary>
        public string attrName;

        /// <summary>
        /// The value of the attribute for the manifest tag.
        /// </summary>
        public string attrValue;

        /// <summary>
        /// An array of additional attributes for the manifest tag.
        /// </summary>
        public string[] attrs;

        /// <summary>
        /// Indicates whether this manifest tag is required.
        /// If false, the tag will be removed from the manifest if found.
        /// </summary>
        public bool required = true;

        public ManifestTagInfo() { }

        public ManifestTagInfo(string nodePath, string tag, string attrName, string attrValue,
                               string[] attrs = null, bool required = true)
        {
            this.nodePath = nodePath;
            this.tag = tag;
            this.attrName = attrName;
            this.attrValue = attrValue;
            this.attrs = attrs;
            this.required = required;
        }
    }
}