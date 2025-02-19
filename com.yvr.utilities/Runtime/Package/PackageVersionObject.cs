using System.Collections.Generic;
using UnityEngine;

namespace YVR.Utilities
{
    [System.Serializable]
    public class PackageVersionObject : ScriptableObject
    {
        public List<PackageInfo> packages;
    }
}