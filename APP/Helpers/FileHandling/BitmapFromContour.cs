using APP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace APP.Helpers.FileHandling
{
    class BitmapFromContour
    {
        /// <summary>
        /// Otrzymujemy Bitmape z Konturu.
        /// </summary>
        /// <param name="contour">Przyjmuje parametr instancji Contour</param>
        /// <returns> Zwraca Bitmape</returns>
        /// Kamil
        public static Bitmap Result(Contour contour)
        {
            Bitmap result = new Bitmap(contour.Width, contour.Height);
            foreach (var item in contour.ContourSet)
            {
                result.SetPixel(item.Location.X, item.Location.Y, item.Type.Color.ToDrawingColor());
            }
            if (contour.ContourSet.Count<=0)
            {
                MessageBoxResult msg =
     MessageBox.Show("Niestety złe kolory, elo.",
     "napis jakis", MessageBoxButton.OK, MessageBoxImage.Question);
            }
        

            return result;

        }
    }
}
