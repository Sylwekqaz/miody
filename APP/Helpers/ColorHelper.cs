using System;
using System.Data.Common;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace APP.Helpers
{
    /// <summary>
    /// Klasa statycznych i rozszerzających funkcji pomocniczych do operacji na kolorach
    /// </summary>
    public static class ColorHelper
        
    {
        public static DColor ToDrawingColor(this MColor color)
        {
            return DColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Dokonuje konwersji Drawing Color na Media Color
        /// </summary>
        /// <param name="color">parametr typu System.Drawing.Color</param>
        /// <returns>Zwraca obiekt typu System.Windows.Media.Color</returns>
        /// Kamil

        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }


        /// <summary>
        /// Oblicza dystans pomiędzy kolorami
        /// </summary>
        /// <param name="r1">wartość składowej czerwonej dla koloru pierwszego</param>
        /// <param name="g1">wartość składowej zielonej dla koloru pierwszego</param>
        /// <param name="b1">wartosć składowej niebieskiej dla koloru pierwszego</param>
        /// <param name="r2">wartość składowej czerwonej dla koloru drugiego</param>
        /// <param name="g2">wartość składowej zielonej dla koloru drugiego</param>
        /// <param name="b2">wartosć składowej niebieskiej dla koloru trzeciego</param>
        /// <returns>Odległość (wartość z przedziału 0 - 442)</returns>
        public static double GetDistance(double r1 , double g1 , double b1 , double r2, double g2 , double b2)
        {
            // compute the Euclidean distance between the two colors
            // note, that the alpha-component is not used in this example
            double dr = r1 - r2;
            double dg = g1 - g2;
            double db = b1 - b2;

            dr = dr*dr;
            dg = dg*dg;
            db = db*db;
            // it is not necessary to compute the square root
            // it should be sufficient to use:
            // temp = dbl_test_blue + dbl_test_green + dbl_test_red;
            // if you plan to do so, the distance should be initialized by 250000.0
            return dr + dg + db;


            //double d1 =color1.GetHue() - color2.GetHue();
            //double d2 = color1.GetSaturation() - color2.GetSaturation();
            //double d3 = color1.GetBrightness() - color2.GetBrightness();

            ////double d = Math.Pow(d1, 2) + Math.Pow(d2, 2) + Math.Pow(d3, 2);
            ////return Math.Sqrt(d);
            //return d1 + d2 + d3;
        }
        /// <summary>
        /// Oblicza dystans między kolorami
        /// </summary>
        /// <param name="color1">obiekt typu System.Windows.Media.Color</param>
        /// <param name="color2">obiekt typu System.Windows.Media.Color</param>
        /// <returns>Odległość (wartość z przedziału 0 - 442)</returns>
        public static double GetDistance(this MColor color1, MColor color2)
        {
            return GetDistance(color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        }
        /// <summary>
        /// Oblicza dystans między kolorami
        /// </summary>
        /// <param name="color1">obiekt typu System.Drawing.Color</param>
        /// <param name="color2">obiekt typu System.Windows.Media.Color</param>
        /// <returns>Odległość (wartość z przedziału 0 - 442)</returns>
        public static double GetDistance(this DColor color1, MColor color2)
        {
            return GetDistance(color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        }

        /// <summary>
        /// Oblicza dystans między kolorami
        /// </summary>
        /// <param name="color1">obiekt typu System.Windows.Media.Color</param>
        /// <param name="color2">obiekt typu System.Drawing.Color</param>
        /// <returns>Odległość (wartość z przedziału 0 - 442)</returns>
        public static double GetDistance(this MColor color1, DColor color2)
        {
            return GetDistance(color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        }

        /// <summary>
        /// Oblicza dystans między kolorami
        /// </summary>
        /// <param name="color1">obiekt typu System.Drawing.Color</param>
        /// <param name="color2">obiekt typu System.Drawing.Color</param>
        /// <returns>Odległość (wartość z przedziału 0 - 442)</returns>
        public static double GetDistance(this DColor color1, DColor color2)
        {
            return GetDistance(color1.R, color1.G, color1.B, color2.R, color2.G, color2.B);
        }
    }
}