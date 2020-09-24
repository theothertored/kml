using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Models
{
    public class KShapeModule : KBasicModule
    {
        public static KShapeModule TryCreateFromModuleNode(XmlNode moduleNode)
        {
            if (moduleNode.Name != "shape") return null;

            var shape = new KShapeModule();
            shape.LoadBasicsFromNode(moduleNode);

            shape.Properties.Add("internal_type", "ShapeModule");

            shape.AddOptional(moduleNode, "shape", "shape_type");
            shape.AddOptional<double>(moduleNode, "width", "shape_width");
            shape.AddOptional<double>(moduleNode, "height", "shape_height");

            shape.AddOptional<double>(moduleNode, "angle", "shape_angle");

            return shape;
        }
    }
}
