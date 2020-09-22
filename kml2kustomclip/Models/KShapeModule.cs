using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip.Models
{
    internal class KShapeModule : KModule
    {
        internal static KShapeModule TryCreateFromModuleNode(XmlNode moduleNode)
        {
            if (moduleNode.Name != "shape") return null;

            var shape = new KShapeModule();

            shape.Properties.Add("internal_type", "ShapeModule");
            shape.AddRequired(moduleNode, "name", "internal_title");

            shape.AddOptional(moduleNode, "shape", "shape_type");
            shape.AddOptional<double>(moduleNode, "width", "shape_width");
            shape.AddOptional<double>(moduleNode, "height", "shape_height");

            shape.AddOptional<double>(moduleNode, "angle", "shape_angle");

            var paintNode = moduleNode.SelectSingleNode("paint");
            if (paintNode != null)
            {
                shape.AddOptional(paintNode, "style", "paint_style");
                shape.AddOptional(paintNode, "color", "paint_color");
                shape.AddOptional<double>(paintNode, "strokewidth", "paint_stroke");
            }

            var positionNode = moduleNode.SelectSingleNode("position");
            if (positionNode != null)
            {
                shape.AddOptional(positionNode, "anchor", "position_anchor");
                shape.AddOptional<double>(positionNode, "xoffset", "position_offset_x");
                shape.AddOptional<double>(positionNode, "yoffset", "position_offset_y");
            }

            var rotationNode = moduleNode.SelectSingleNode("rotation");
            if (rotationNode != null)
            {
                shape.AddRequired(rotationNode, "mode", "shape_rotate_mode");
                shape.AddOptional<double>(rotationNode, "offset", "shape_rotate_offset");
                shape.AddOptional<double>(rotationNode, "radius", "shape_rotate_radius");
            }

            var maskNode = moduleNode.SelectSingleNode("mask");
            if (maskNode != null)
            {
                shape.AddRequired(maskNode, "type", "fx_mask");
                shape.AddOptional<double>(maskNode, "blur", "fx_bitmap_blur");
                shape.AddOptional<double>(maskNode, "dim", "fx_bitmap_dim");
                shape.AddOptional<string>(maskNode, "filter", "fx_bitmap_filter");
                shape.AddOptional<double>(maskNode, "filteramount", "fx_bitmap_filter_amount");
            }

            var textureNode = moduleNode.SelectSingleNode("texture");
            if (textureNode != null)
            {
                shape.AddRequired(textureNode, "type", "fx_gradient");

                shape.AddOptional<double>(textureNode, "offset", "fx_gradient_offset");
                shape.AddOptional<double>(textureNode, "width", "fx_gradient_width");
                shape.AddOptional<string>(textureNode, "color", "fx_gradient_color");
                shape.AddOptional<double>(textureNode, "centerx", "fx_gradient_offset_x");
                shape.AddOptional<double>(textureNode, "centery", "fx_gradient_offset_y");

                shape.AddOptional<string>(textureNode, "bitmap", "fx_bitmap");
                shape.AddOptional<double>(textureNode, "bitmapblur", "fx_bitmap_blur");
                shape.AddOptional<double>(textureNode, "bitmapdim", "fx_bitmap_dim");
                shape.AddOptional<string>(textureNode, "bitmapfilter", "fx_bitmap_filter");
                shape.AddOptional<double>(textureNode, "bitmapfilteramount", "fx_bitmap_filter_amount");
                shape.AddOptional<double>(textureNode, "bitmapwidth", "fx_gradient_bitmap_width");
            }

            var animationsNode = moduleNode.SelectSingleNode("animations");
            if (animationsNode != null)
            {
                var animationNodes = animationsNode.SelectNodes("animation");
                foreach (XmlNode animationNode in animationNodes)
                    shape.InternalAnimations.Add(KAnimation.CreateFromModuleNode(animationNode));
            }

            return shape;
        }
    }
}
