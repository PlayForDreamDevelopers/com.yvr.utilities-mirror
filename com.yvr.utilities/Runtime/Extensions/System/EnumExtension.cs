using System;
using System.ComponentModel;

namespace YVR.Utilities
{
    public static class EnumExtension
    {
        /// <summary>
        /// Get the description in the DescriptionAttribute
        /// </summary>
        /// <param name="target"> enum </param>
        /// <returns></returns>
        public static string GetDescription(this Enum target)
        {
            var description = target.ToString();
            var fieldInfo = target.GetType().GetField(target.ToString());

            if (fieldInfo != null)
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0)
                {
                    description = ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return description;
        }
    }
}