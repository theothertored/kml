using kml2kustomclip.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Models
{
    internal class KAnimation : KModule
    {
        public List<KAnimationKeyframe> Animator { get; } = new List<KAnimationKeyframe>();

        internal static KAnimation CreateFromModuleNode(XmlNode moduleNode)
        {
            if (moduleNode.Name != "animation")
                throw new KmlParseException($"Expected 'animation' node, got '{moduleNode.Name}'.");

            var animation = new KAnimation();

            animation.AddRequired(moduleNode, "reacton", "type");
            animation.AddRequired(moduleNode, "action", "action");
            animation.AddOptional(moduleNode, "ease", "ease");
            animation.AddOptional(moduleNode, "anchor", "anchor");
            animation.AddOptional(moduleNode, "rule", "rule");
            animation.AddOptional(moduleNode, "center", "center");
            animation.AddOptional(moduleNode, "filter", "filter");
            animation.AddOptional<double>(moduleNode, "duration", "duration");
            animation.AddOptional<double>(moduleNode, "amount", "amount");
            animation.AddOptional<double>(moduleNode, "speed", "speed");
            animation.AddOptional<double>(moduleNode, "delay", "delay");
            animation.AddOptional<double>(moduleNode, "angle", "angle");
            animation.AddOptional<double>(moduleNode, "trigger", "trigger");

            var keyframeNodes = moduleNode.SelectNodes("keyframe");
            foreach (XmlNode keyframeNode in keyframeNodes)
                animation.Animator.Add(KAnimationKeyframe.CreateFromNode(keyframeNode));

            return animation;
        }
    }
}
