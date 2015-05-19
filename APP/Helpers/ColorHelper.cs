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

        public static double GetDistance(this DColor color1, DColor color2)
        {
            double d1 =color1.GetHue() - color2.GetHue();
            double d2 = color1.GetSaturation() - color2.GetSaturation();
            double d3 = color1.GetBrightness() - color2.GetBrightness();

            //double d = Math.Pow(d1, 2) + Math.Pow(d2, 2) + Math.Pow(d3, 2);
            //return Math.Sqrt(d);
            return d1 + d2 + d3;
        }

        public static double GetDistance(this MColor color1, MColor color2)
        {
            return color1.ToDrawingColor().GetDistance(color2.ToDrawingColor());
        }

        public static double GetDistance(this DColor color1, MColor color2)
        {
          return color1.GetDistance(color2.ToDrawingColor());   
        }

        public static double GetDistance(this MColor color1, DColor color2)
        {
            return color1.ToDrawingColor().GetDistance(color2);
        }
    }
}