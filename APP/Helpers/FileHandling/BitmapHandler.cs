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

#if DEBUG
            Bitmap tempBitmap = new Bitmap(bitmap.Width,bitmap.Height);
            for (int w = 0; w < bitmap.Width; w++)
            {
                for (int h = 0; h < bitmap.Height; h++)
                {
                    tempBitmap.SetPixel(w,h,System.Drawing.Color.White);
                }
            }

#endif


            Contour wynikContour = new Contour(bitmap.Width, bitmap.Height);
            wynikContour.Bitmap = bitmap;
            for (int i = 0; i < bitmap.Height; i++) 
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var drawingColor = bitmap.GetPixel(j, i);
                    Color pixelcolor = Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
                    if (Pollen.TryPrase(pixelcolor) != null)
                    {
                        ContourPoint point = new ContourPoint()
                        {
                            Location = new Point(j, i),
                            Type = Pollen.TryPrase(pixelcolor)
                        };
#if DEBUG
                        tempBitmap.SetPixel(j, i, point.Type.Color.ToDrawingColor());
#endif
                        wynikContour.ContourSet.Add(point);
                    }
                   
                }
            }
            return wynikContour;
        }
    }
}
