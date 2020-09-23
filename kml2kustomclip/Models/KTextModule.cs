using kml2kustomclip.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Models
{
    public class KTextModule : KBasicModule
    {
        public List<string> TextFilter { get; } = new List<string>();

        public static KTextModule TryCreateFromModuleNode(XmlNode moduleNode)
        {
            if (moduleNode.Name != "text") return null;

            var text = new KTextModule();
            text.LoadBasicsFromNode(moduleNode);

            text.Properties.Add("internal_type", "TextModule");

            text.AddRequired(moduleNode, "font", "text_family");
            text.AddRequired(moduleNode, "align", "text_align");
            text.AddRequired(moduleNode, "width", "text_width");
            text.AddRequired(moduleNode, "height", "text_height");
            text.AddRequired(moduleNode, "maxlines", "text_lines");
            text.AddRequired(moduleNode, "size", "text_size");
            text.AddRequired(moduleNode, "type", "text_size_type");

            var textNode = moduleNode.SelectSingleNode("text");
            if (textNode == null) throw KmlParseException.MissingRequiredNode("text", moduleNode);
            text.AddFromContent(textNode, "text_expression", text.Properties);

            var textFiltersNode = moduleNode.SelectSingleNode("textfilters");
            if (textFiltersNode != null)
            {
                var filterNodes = textFiltersNode.SelectNodes("filter");
                foreach (XmlNode filterNode in filterNodes)
                {
                    text.TextFilter.Add(filterNode.InnerText);
                }
            }

            return text;
        }
    }
}
