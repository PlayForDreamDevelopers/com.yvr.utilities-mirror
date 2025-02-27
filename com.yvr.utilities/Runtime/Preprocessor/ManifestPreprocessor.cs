using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace YVR.Utilities
{
    /// <summary>
    /// A static class containing methods to patch an AndroidManifest.xml file.
    /// </summary>
    public static class ManifestPreprocessor
    {
        /// <summary>
        /// Applies the given manifestTagInfos to the specified AndroidManifest.xml file.
        /// </summary>
        /// <param name="manifestTagInfos">A list of ManifestTagInfos to be applied to the AndroidManifest.xml file.</param>
        /// <param name="sourceFile">The path to the source AndroidManifest.xml file.</param>
        /// <param name="destinationFile">The path to the destination AndroidManifest.xml file. If null, the sourceFile will be overwritten.</param>
        public static void PatchAndroidManifest(List<ManifestTagInfo> manifestTagInfos, string sourceFile,
                                                string destinationFile = null)
        {
            // If no destinationFile is provided, use the sourceFile as the destinationFile
            destinationFile ??= sourceFile;

            try
            {
                // Load the AndroidManifest.xml file
                XmlDocument doc = new XmlDocument();
                doc.Load(sourceFile);

                // Find the manifest tag in the XML document
                XmlElement manifestNode = (XmlElement) doc.SelectSingleNode("/manifest");
                if (manifestNode == null)
                    throw new NullReferenceException($"The '/manifest' element could not found in {sourceFile}");

                // Get the Android namespace URI from the manifest tag
                string namespaceUri = manifestNode.GetAttribute("xmlns:android");
                if (string.IsNullOrEmpty(namespaceUri))
                    throw new NullReferenceException($"The 'xmlns:android' attribute could not found in {sourceFile}");

                // Apply each ManifestTagInfo to the XML document
                manifestTagInfos.ForEach(tagInfo =>
                {
                    if (tagInfo.required)
                        AddOrModifyTag(doc, namespaceUri, tagInfo.nodePath, tagInfo.tag,
                                       tagInfo.attrName, tagInfo.attrValue, tagInfo.modifyIfFound,
                                       tagInfo.attrs);
                    else if (tagInfo.modifyIfFound)
                        RemoveTag(doc, namespaceUri, tagInfo.nodePath, tagInfo.tag, tagInfo.attrName,
                                  tagInfo.attrValue);
                });

                // Save the modified XML document to the destination file
                doc.Save(destinationFile);
            } catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        #region internal

        private static void RemoveTag(XmlDocument doc, string namespaceUri, string nodePath, string tag,
                                      string attrName, string attrValue) // name, value pairs
        {
            XmlElement element = GetXmlElement(doc, nodePath, tag, attrValue, attrName, namespaceUri);
            element?.ParentNode?.RemoveChild(element);
        }

        private static XmlNode EnsureNodePath(XmlDocument doc, string nodePath)
        {
            var currentNode = doc as XmlNode;
            var pathParts = nodePath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in pathParts)
            {
                var nextNode = currentNode.SelectSingleNode(part);
                if (nextNode == null)
                {
                    nextNode = doc.CreateElement(part);
                    currentNode.AppendChild(nextNode);
                }

                currentNode = nextNode;
            }

            return currentNode;
        }

        private static void AddOrModifyTag(XmlDocument doc, string namespaceUri, string nodePath, string tag,
                                           string attrName, string attrValue, bool modifyIfFound,
                                           params string[] attrs) // name, value pairs
        {
            XmlElement element = GetXmlElement(doc, nodePath, tag, attrValue, attrName, namespaceUri);
            if (element == null)
            {
                var parent = EnsureNodePath(doc, nodePath);

                var tagParts = tag.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                var previousElement = parent;
                for (int i = 0; i != tagParts.Length; ++i)
                {
                    element = doc.CreateElement(tagParts[i]);
                    if (i == tagParts.Length - 1)
                    {
                        if (!string.IsNullOrEmpty(attrName))
                            element.SetAttribute(attrName, namespaceUri, attrValue);
                    }

                    previousElement.AppendChild(element);
                    previousElement = element;
                }
            }

            //return when attrs is null
            if (attrs == null)
                return;

            //add or remove attrs
            for (int i = 0; i < attrs.Length; i += 2)
            {
                if (modifyIfFound || string.IsNullOrEmpty(element.GetAttribute(attrs[i], namespaceUri)))
                {
                    if (attrs[i + 1] != null)
                    {
                        element.SetAttribute(attrs[i], namespaceUri, attrs[i + 1]);
                    }
                    else
                    {
                        element.RemoveAttribute(attrs[i], namespaceUri);
                    }
                }
            }
        }

        private static XmlElement GetXmlElement(XmlDocument doc, string nodePath, string tag, string name,
                                                string localName, string namespaceUri)
        {
            // Find the XML node specified by the nodePath
            var nodes = doc.SelectNodes($"{nodePath}/{tag}");
            if (nodes == null)
                throw new NullReferenceException($"The '{nodePath}/{tag}' can not found in {doc.Name}");
            return nodes.Cast<XmlElement>().FirstOrDefault(node =>
                                                               string.IsNullOrEmpty(name) ||
                                                               name == node.GetAttribute(localName, namespaceUri));
        }

        #endregion
    }
}