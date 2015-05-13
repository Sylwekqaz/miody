using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace APP.Helpers
{
    public static class ColorHelper
    {
        public static DColor ToDrawingColor(this MColor color)
        {
            return DColor.FromArgb(color.A, color.R, color.G, color.B);
        }


        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static float GetDistance(this DColor color1, DColor color2)
        {
            float d1 = Math.Abs(color1.GetHue() - color2.GetHue());
            float d2 = Math.Abs(color1.GetSaturation() - color2.GetSaturation());
            float d3 = Math.Abs(color1.GetBrightness() - color2.GetBrightness());

            return d1 + d2 + d3;
        }

        public static float GetDistance(this MColor color1, MColor color2)
        {
            return color1.ToDrawingColor().GetDistance(color2.ToDrawingColor());
        }

        public static float GetDistance(this DColor color1, MColor color2)
        {
          return color1.GetDistance(color2.ToDrawingColor());   
        }

        public static float GetDistance(this MColor color1, DColor color2)
        {
            return color1.ToDrawingColor().GetDistance(color2);
        }
    }
}