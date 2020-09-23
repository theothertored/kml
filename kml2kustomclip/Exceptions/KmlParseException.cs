using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Exceptions
{

    [Serializable]
    public class KmlParseException : Exception
    {
        public KmlParseException() { }
        public KmlParseException(string message) : base(message) { }
        public KmlParseException(string message, Exception inner) : base(message, inner) { }
        protected KmlParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public static KmlParseException MissingRequiredAttribute(string attrName, XmlNode node)
            => new KmlParseException($"Missing required attribute '{attrName}' on node '{node.Name}'.");

        public static Exception MissingRequiredNode(string nodeName, XmlNode moduleNode)
            => new KmlParseException($"Missing required attribute '{nodeName}' in node '{moduleNode.Name}'.");

        public static KmlParseException CannotSetToFormulaAndGlobal(string attrName, XmlNode node)
            => new KmlParseException($"Cannot set attribute '{attrName}' of node '{node.Name}' to a formula and a global at the same time.");

        public static KmlParseException CouldNotParseValueAsType(string value, Type type)
            => new KmlParseException($"Could not parse value '{value}' as type '{type.FullName}'.");

        public static KmlParseException InvalidRepeatValue(string value)
            => new KmlParseException($"Invalid repeat value '{value}'.");

        public static KmlParseException UnknownModuleName(string name)
            => new KmlParseException($"Unknown module name '{name}'.");

        public static KmlParseException ExpectedDifferentNode(string expected, XmlNode real)
            => new KmlParseException($"Expected node '{expected}', got '{real.Name}'.");
    }
}
