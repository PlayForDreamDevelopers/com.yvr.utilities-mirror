#if UNITY_EDITOR
#endif
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YVR.Utilities
{
    public class PackageVersion
    {
        public const string assetPath = "/XR/Resources";
        public const string assetName = nameof(PackageVersion);

        /// <summary>
        /// Get the entire package store list.
        /// </summary>
        /// <returns>The list of stored package information</returns>
        public static List<PackageInfo> GetPackages()
        {
            var packageVersion = Resources.Load<PackageVersionObject>(assetName);
            if (packageVersion == null)
            {
                Debug.LogError($"can not load {assetName}");
                return null;
            }

            return packageVersion.packages;
        }

        /// <summary>
        /// Get the version of the specfiied package name/ assembly name.
        /// </summary>
        /// <returns>Whether the specified package is found</returns>
        public static bool TryGetPackageVersion(string targetPackageNameOrAssemblyName, out string version)
        {
            bool result = false;
            string targetVersion = null;
            var packageList = GetPackages();
            if (packageList != null)
            {
                packageList.ForEach(package =>
                {
                    if (package.Name.Equals(targetPackageNameOrAssemblyName) ||
                        targetPackageNameOrAssemblyName.Equals(package.AssemblyName))
                    {
                        targetVersion = package.Version;
                        result = true;
                    }
                });
            }

            version = targetVersion;
            return result;
        }

        /// <summary>
        /// Print the version of the package with the specified name, if not specified, print all the packages
        /// </summary>
        /// <returns>Whether the specified package is found</returns>
        public static bool PrintPackagesVersion(string targetPackageNameOrAssemblyName = null)
        {
            var packageList = GetPackages();
            var result = false;
            if (packageList != null)
            {
                packageList.ForEach(package =>
                {
                    if (!(!string.IsNullOrEmpty(targetPackageNameOrAssemblyName) &&
                          !targetPackageNameOrAssemblyName.Equals(package.Name) &&
                          !targetPackageNameOrAssemblyName.Equals(package.AssemblyName)))
                    {
                        Debug.Log($"{package.Name}:{package.Version}");
                        result = true;
                    }
                });
            }

            return result;
        }

        /// <summary>
        ///  Print the version of the package with the type in the package assembly.
        /// </summary>
        /// <param name="type">The type in the package assembly</param>
        /// <returns>Whether the specified package is found</returns>
        public static bool PrintPackagesVersion(Type type)
        {
            return PrintPackagesVersion(type.Assembly.GetName().Name);
        }
    }
}