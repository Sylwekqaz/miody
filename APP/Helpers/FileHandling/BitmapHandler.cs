using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using APP.Model;
using Color = System.Windows.Media.Color;

namespace APP.Helpers.FileHandling
{
    public interface IBitmapHandler
    {
        Contour LoadBitmap(Bitmap bitmap);
    }

    public class BitmapHandler : IBitmapHandler
    {
        /// <summary>
        /// Metoda odczytuje z bitmapy dane, tworząc kontur
        /// </summary>
        /// <param name="bitmap">
        /// Instancja bitmapy, na jednej bitmapie mamy wiele konturow, ktore roznia sie kolorem
        /// </param>
        /// <returns>
        /// Zwraca Kontur
        /// </returns>
        public Contour LoadBitmap(Bitmap bitmap)
        {
            Contour wynikContour = new Contour();
            wynikContour.Bitmap = bitmap;
            for (int i = 0; i < bitmap.Height; i++) 
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var drawindColor = bitmap.GetPixel(j, i);
                    Color pixelcolor = Color.FromArgb(drawindColor.A, drawindColor.R, drawindColor.G, drawindColor.B);
                    if (Pollen.TryPrase(pixelcolor) != null)
                    {
                        ContourPoint point = new ContourPoint()
                        {
                            Location = new Point(i, j),
                            Type = Pollen.TryPrase(pixelcolor)
                        };
                        wynikContour.ContourSet.Add(point);
                    }
                   
                }
            }
       

            return wynikContour;
        }
    }
}
