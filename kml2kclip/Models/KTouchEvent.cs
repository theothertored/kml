using kml2kclip.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kclip.Models
{
    public class KTouchEvent
    {
        public string Type { get; set; }
        public string Action { get; set; }
        public string Switch { get; set; }
        public string SwitchText { get; set; }

        public static KTouchEvent CreateFromEventNode(XmlNode eventNode)
        {
            if (eventNode.Name != "event")
                throw KmlParseException.ExpectedDifferentNode("event", eventNode);

            var ev = new KTouchEvent();

            var typeAttr = eventNode.Attributes["type"];
            if (typeAttr == null) throw KmlParseException.MissingRequiredAttribute("type", eventNode);
            ev.Type = typeAttr.Value;

            var actionAttr = eventNode.Attributes["action"];
            if (actionAttr == null) throw KmlParseException.MissingRequiredAttribute("action", eventNode);
            ev.Action = actionAttr.Value;

            var globalAttr = eventNode.Attributes["global"];
            if (globalAttr != null)
            {
                ev.Switch = globalAttr.Value;
            }

            ev.SwitchText = eventNode.InnerText.Trim();

            return ev;
        }
    }
}
