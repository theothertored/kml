using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kclip.Models
{
    public abstract class KBasicModule : KModule
    {
        protected void LoadBasicsFromNode(XmlNode moduleNode)
        {
            AddRequired(moduleNode, "name", "internal_title");

            var paintNode = moduleNode.SelectSingleNode("paint");
            if (paintNode != null)
            {
                AddOptional(paintNode, "style", "paint_style");
                AddOptional(paintNode, "color", "paint_color");
                AddOptional<double>(paintNode, "strokewidth", "paint_stroke");
            }

            var positionNode = moduleNode.SelectSingleNode("position");
            if (positionNode != null)
            {
                AddOptional(positionNode, "anchor", "position_anchor");
                AddOptional<double>(positionNode, "xoffset", "position_offset_x");
                AddOptional<double>(positionNode, "yoffset", "position_offset_y");
            }

            var rotationNode = moduleNode.SelectSingleNode("rotation");
            if (rotationNode != null)
            {
                AddRequired(rotationNode, "mode", "shape_rotate_mode");
                AddOptional<double>(rotationNode, "offset", "shape_rotate_offset");
                AddOptional<double>(rotationNode, "radius", "shape_rotate_radius");
            }

            var maskNode = moduleNode.SelectSingleNode("mask");
            if (maskNode != null)
            {
                AddRequired(maskNode, "type", "fx_mask");
                AddOptional<double>(maskNode, "blur", "fx_bitmap_blur");
                AddOptional<double>(maskNode, "dim", "fx_bitmap_dim");
                AddOptional<string>(maskNode, "filter", "fx_bitmap_filter");
                AddOptional<double>(maskNode, "filteramount", "fx_bitmap_filter_amount");
            }

            var textureNode = moduleNode.SelectSingleNode("texture");
            if (textureNode != null)
            {
                AddRequired(textureNode, "type", "fx_gradient");

                AddOptional<double>(textureNode, "offset", "fx_gradient_offset");
                AddOptional<double>(textureNode, "width", "fx_gradient_width");
                AddOptional<string>(textureNode, "color", "fx_gradient_color");
                AddOptional<double>(textureNode, "centerx", "fx_gradient_offset_x");
                AddOptional<double>(textureNode, "centery", "fx_gradient_offset_y");

                AddOptional<string>(textureNode, "bitmap", "fx_bitmap");
                AddOptional<double>(textureNode, "bitmapblur", "fx_bitmap_blur");
                AddOptional<double>(textureNode, "bitmapdim", "fx_bitmap_dim");
                AddOptional<string>(textureNode, "bitmapfilter", "fx_bitmap_filter");
                AddOptional<double>(textureNode, "bitmapfilteramount", "fx_bitmap_filter_amount");
                AddOptional<double>(textureNode, "bitmapwidth", "fx_gradient_bitmap_width");
            }

            var animationsNode = moduleNode.SelectSingleNode("animations");
            if (animationsNode != null)
            {
                var animationNodes = animationsNode.SelectNodes("animation");
                foreach (XmlNode animationNode in animationNodes)
                {
                    InternalAnimations.Add(KAnimation.CreateFromModuleNode(animationNode));
                }
            }

            var eventsNode = moduleNode.SelectSingleNode("events");
            if (eventsNode != null)
            {
                var eventNodes = eventsNode.SelectNodes("event");
                foreach (XmlNode eventNode in eventNodes)
                {
                    InternalEvents.Add(KTouchEvent.CreateFromEventNode(eventNode));
                }
            }
        }
    }
}
