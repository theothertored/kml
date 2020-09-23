using kml2kustomclip.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace kml2kustomclip.Models
{
    public class KModule
    {
        [JsonExtensionData]
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public Dictionary<string, object> InternalToggles { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> InternalFormulas { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> InternalGlobals { get; } = new Dictionary<string, object>();
        public List<KAnimation> InternalAnimations { get; } = new List<KAnimation>();

        public bool ShouldSerializeInternalToggles() => InternalToggles.Count > 0;
        public bool ShouldSerializeInternalFormulas() => InternalFormulas.Count > 0;
        public bool ShouldSerializeInternalGlobals() => InternalGlobals.Count > 0;
        public bool ShouldSerializeInternalAnimations() => InternalAnimations.Count > 0;

        /// <summary>
        /// Run <see cref="AddRequired{T}(XmlNode, string, string)"/> with T = string.
        /// </summary>
        /// <param name="node">The node to read attribute value and subnodes from.</param>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="entryName">The key to add to dictionaries.</param>
        protected void AddRequired(XmlNode node, string attrName, string entryName)
            => AddRequired<string>(node, attrName, entryName);

        protected void AddRequired<T>(XmlNode node, string attrName, string entryName)
        {
            AddAttrValueFormulaGlobal<T>(node, attrName, entryName, true);
        }

        /// <summary>
        /// Run <see cref="AddOptional{T}(XmlNode, string, string)"/> with T = string.
        /// </summary>
        protected void AddOptional(XmlNode node, string attrName, string entryName)
            => AddOptional<string>(node, attrName, entryName);

        protected void AddOptional<T>(XmlNode node, string attrName, string entryName)
        {
            AddAttrValueFormulaGlobal<T>(node, attrName, entryName, false);
        }

        /// <summary>
        /// Read the value of <paramref name="attrName"/> of <paramref name="node"/>,
        /// parse it to the given type using <see cref="ParseValue{T}(string)"/>,
        /// then add the value to <see cref="Properties"></see>.<br/>
        /// After that, search <paramref name="node"/> for a formula or global node with attr = <paramref name="attrName"/>.
        /// If found, add formulas to <see cref="InternalFormulas"/> and globals to <see cref="InternalGlobals"/>,
        /// then set <see cref="InternalToggles"/>[<paramref name="entryName"/>] to 0 for normal value, 10 for formula and 100 for global.<br/>
        /// Throws when <paramref name="attrName"/> value cannot be parsed or when the attribute is not found and <paramref name="throwIfMissing"/> is true. 
        /// </summary>
        /// <typeparam name="T">The type to convert the attribute value to.</typeparam>
        /// <param name="node">The node to read attribute value and subnodes from.</param>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="entryName">The key to add to dictionaries.</param>
        /// <exception cref="KmlParseException"></exception>
        protected void AddAttrValueFormulaGlobal<T>(XmlNode node, string attrName, string entryName, bool throwIfMissing)
        {
            var attr = node.Attributes[attrName];
            var globalNode = node.SelectSingleNode($"global[@attr='{attrName}']");
            var formulaNode = node.SelectSingleNode($"formula[@attr='{attrName}']");

            if (attr == null)
            {
                if (throwIfMissing)
                    throw KmlParseException.MissingRequiredAttribute(attrName, node);
                else
                    return;
            }

            if (attr != null)
                Properties.Add(entryName, ParseValue<T>(attr.Value));

            if (globalNode != null && formulaNode != null)
                throw KmlParseException.CannotSetToFormulaAndGlobal(attrName, node);

            if (globalNode != null)
            {
                InternalToggles.Add(entryName, 100);
                AddFromAttribute(globalNode, "global", entryName, true, InternalGlobals);
            }

            if (formulaNode != null)
            {
                InternalToggles.Add(entryName, 10);
                AddFromContent(formulaNode, entryName, InternalFormulas);
            }
        }

        #region helpers

        protected void AddFromAttribute(XmlNode node, string attrName, string entryName, bool throwIfMissing, Dictionary<string, object> dict = null)
            => AddFromAttribute<string>(node, attrName, entryName, throwIfMissing, dict);

        protected void AddFromAttribute<T>(XmlNode node, string attrName, string entryName, bool throwIfMissing, Dictionary<string, object> dict = null)
        {
            var attr = node.Attributes[attrName];

            if (attr == null)
            {
                if (throwIfMissing)
                    throw KmlParseException.MissingRequiredAttribute(attrName, node);
                else
                    return;
            }

            if (dict == null)
                Properties.Add(entryName, ParseValue<T>(attr.Value));
            else
                dict.Add(entryName, ParseValue<T>(attr.Value));
        }

        protected void AddFromContent(XmlNode node, string entryName, Dictionary<string, object> dict = null)
        {
            var text = node.InnerText.Trim();

            if (dict == null)
                Properties.Add(entryName, text);
            else
                dict.Add(entryName, text);
        }

        protected object ParseValue<T>(string value)
        {
            var type = typeof(T);

            try
            {
                // short-circuit here for strings
                if (type == typeof(string))
                    return value;

                if (type == typeof(double))
                    return double.Parse(value);

                if (type == typeof(int))
                    return int.Parse(value);

                if (type == typeof(float))
                    return float.Parse(value);

                else
                    return value;
            }
            catch (Exception ex)
            {
                throw KmlParseException.CouldNotParseValueAsType(value, type);
            }
        }

        #endregion

        #region static xml node to submodule methods

        public static List<KModule> SubmodulesNodeToKModuleList(XmlNode submodulesNode)
        {
            var list = new List<KModule>();

            foreach (XmlNode node in submodulesNode.ChildNodes)
            {
                // ignore comment nodes
                if (node.NodeType == XmlNodeType.Comment) continue;

                var repeatAttr = node.Attributes["repeat"];

                if (repeatAttr != null)
                {
                    int repeatCount;
                    if (int.TryParse(repeatAttr.Value, out repeatCount))
                    {
                        // add multiple times
                        for (int i = 0; i < repeatCount; i++)
                            list.Add(ModuleNodeToKModule(node));
                    }
                    else
                    {
                        throw KmlParseException.InvalidRepeatValue(repeatAttr.Value);
                    }
                }
                else
                {
                    // add once
                    list.Add(ModuleNodeToKModule(node));
                }

            }

            return list;
        }

        public static KModule ModuleNodeToKModule(XmlNode moduleNode)
        {
            var node = KShapeModule.TryCreateFromModuleNode(moduleNode);

            if (node == null) throw KmlParseException.UnknownModuleName(moduleNode.Name);

            return node;
        }

        #endregion
    }
}
