using System.Collections.Generic;
using UnityEngine;

namespace YVR.Utilities
{
    public abstract class YVRProjectSettingSO : ScriptableObject
    {
        public static List<YVRProjectSettingSO> projectSettingsList = new();
        public virtual bool configurable => true;
    }
}