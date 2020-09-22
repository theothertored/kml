using kml2kustomclip.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Models
{
    public class KAnimationKeyframe
    {
        public string Property { get; set; }
        public string Ease { get; set; }
        public double Position { get; set; }
        public double Value { get; set; }

        public static KAnimationKeyframe CreateFromNode(XmlNode node)
        {
            if (node.Name != "keyframe")
                throw KmlParseException.ExpectedDifferentNode("keyframe", node);

            var keyframe = new KAnimationKeyframe();

            var propertyAttr = node.Attributes["property"];
            if (propertyAttr == null)
                throw KmlParseException.MissingRequiredAttribute("property", node);
            else
                keyframe.Property = propertyAttr.Value;

            double dbl;

            var positionAttr = node.Attributes["position"];
            if (positionAttr == null)
                throw KmlParseException.MissingRequiredAttribute("position", node);
            else
            {
                if (double.TryParse(positionAttr.Value, out dbl))
                    keyframe.Position = dbl;
                else
                    throw KmlParseException.CouldNotParseValueAsType(positionAttr.Value, typeof(double));
            }

            var valueAttr = node.Attributes["value"];
            if (valueAttr == null)
                throw KmlParseException.MissingRequiredAttribute("value", node);
            else
            {
                if (double.TryParse(valueAttr.Value, out dbl))
                    keyframe.Value = dbl;
                else
                    throw KmlParseException.CouldNotParseValueAsType(valueAttr.Value, typeof(double));
            }

            // ease is the only non-required attribute here
            var easeAttr = node.Attributes["ease"];
            if (easeAttr != null)
                keyframe.Ease = easeAttr.Value;

            return keyframe;
        }
    }
}
