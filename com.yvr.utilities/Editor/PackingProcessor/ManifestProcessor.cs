using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace YVR.Utilities.Editor.PackingProcessor
{
    /// <summary>
    /// A static class containing methods to patch an AndroidManifest.xml file.
    /// </summary>
    public static class ManifestProcessor
    {
        private static string s_NamespaceUri = null;

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
                var doc = new XmlDocument();
                doc.Load(sourceFile);

                // Find the manifest tag in the XML document
                var manifestNode = (XmlElement) doc.SelectSingleNode("/manifest");
                if (manifestNode == null)
                    throw new NullReferenceException($"The '/manifest' element could not found in {sourceFile}");

                // Get the Android namespace URI from the manifest tag
                s_NamespaceUri ??= manifestNode.GetAttribute("xmlns:android");
                if (string.IsNullOrEmpty(s_NamespaceUri))
                    throw new NullReferenceException($"The 'xmlns:android' attribute could not found in {sourceFile}");

                // Apply each ManifestTagInfo to the XML document
                manifestTagInfos.ForEach(tagInfo =>
                {
                    if (tagInfo.required)
                    {
                        AddOrModifyTag(doc, tagInfo.nodePath, tagInfo.tag, tagInfo.attrName,
                                       tagInfo.attrValue, tagInfo.attrs);
                    }
                    else
                    {
                        RemoveTag(doc, tagInfo.nodePath, tagInfo.tag, tagInfo.attrName, tagInfo.attrValue);
                    }
                });

                // Save the modified XML document to the destination file
                doc.Save(destinationFile);
            } catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        private static void RemoveTag(XmlNode doc, string nodePath, string tag,
                                      string attrName, string attrValue) // name, value pairs
        {
            XmlElement element = GetXmlElement(doc, nodePath, tag, attrValue, attrName);
            element?.ParentNode?.RemoveChild(element);
        }

        private static XmlNode EnsureNodePath(XmlDocument doc, string nodePath)
        {
            var currentNode = doc as XmlNode;
            string[] pathParts = nodePath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in pathParts)
            {
                XmlNode nextNode = currentNode.SelectSingleNode(part);
                if (nextNode == null)
                {
                    nextNode = doc.CreateElement(part);
                    currentNode.AppendChild(nextNode);
                }

                currentNode = nextNode;
            }

            return currentNode;
        }

        private static void AddOrModifyTag(XmlDocument doc, string nodePath, string tag,
                                           string attrName, string attrValue,
                                           params string[] attrs) // name, value pairs
        {
            XmlElement element = GetXmlElement(doc, nodePath, tag, attrValue, attrName);
            if (element != null)
            {
                XmlNode parentNode = element.ParentNode;
                RemoveTag(doc, nodePath, tag, attrName, attrValue);
                element = null;

                // Avoid empty nodes
                if (!parentNode.HasChildNodes)
                {
                    parentNode.ParentNode?.RemoveChild(parentNode);
                }
            }

            XmlNode parent = EnsureNodePath(doc, nodePath);

            string[] tagParts = tag.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            XmlNode previousElement = parent;
            for (int i = 0; i != tagParts.Length; ++i)
            {
                element = doc.CreateElement(tagParts[i]);
                if (i == tagParts.Length - 1)
                {
                    if (!string.IsNullOrEmpty(attrName))
                        element.SetAttribute(attrName, s_NamespaceUri, attrValue);
                }

                previousElement.AppendChild(element);
                previousElement = element;
            }

            if (attrs == null) return;

            for (int i = 0; i < attrs.Length; i += 2)
            {
                if (!string.IsNullOrEmpty(element.GetAttribute(attrs[i], s_NamespaceUri))) continue;
                if (attrs[i + 1] != null)
                    element.SetAttribute(attrs[i], s_NamespaceUri, attrs[i + 1]);
                else
                    element.RemoveAttribute(attrs[i], s_NamespaceUri);
            }
        }

        private static XmlElement GetXmlElement(XmlNode doc, string nodePath, string tag, string attrValue,
                                                string attrName)
        {
            // Find the XML node specified by the nodePath
            XmlNodeList nodes = doc.SelectNodes($"{nodePath}/{tag}");
            if (nodes == null)
                throw new NullReferenceException($"The '{nodePath}/{tag}' can not found in {doc.Name}");
            return nodes.Cast<XmlElement>().FirstOrDefault(node => string.IsNullOrEmpty(attrValue) ||
                                                                   attrValue ==
                                                                   node.GetAttribute(attrName, s_NamespaceUri));
        }
    }
}