using System.Drawing;
using APP.Model;

namespace APP.Helpers.FileHandling
{
    public interface IBitmapHandler
    {
        Contour LoadBitmap(Bitmap bitmap);
    }

    public class BitmapHandler : IBitmapHandler
    {
        private IErrorLog Log;
        private bool _error = false;

        public BitmapHandler(IErrorLog log)
        {
            Log = log;
        }

        /// <summary>
        /// Metoda odczytuje z bitmapy dane, tworząc kontur
        /// </summary>
        /// <param name="bitmap">
        /// Instancja bitmapy, na jednej bitmapie mamy wiele konturow, ktore roznia sie kolorem
        /// </param>
        /// <returns>
        /// Zwraca Kontur
        /// </returns>
        /// Kamil
        public Contour LoadBitmap(Bitmap bitmap)
        {

            Bitmap contourBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            contourBitmap.MakeTransparent();
            contourBitmap.Clear(Color.Transparent);




            Contour wynikContour = new Contour(bitmap.Width, bitmap.Height)
            {
                Bitmap = bitmap
            };

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var drawingColor = bitmap.GetPixel(j, i);

                    var pollen = Pollen.TryPrase(drawingColor.ToMediaColor());
                    if (pollen != null)
                    {
                        ContourPoint point = new ContourPoint
                        {
                            Location = new Point(j, i),
                            Type = pollen
                        };

                        contourBitmap.SetPixel(j, i, point.Type.Color.ToDrawingColor());

                        wynikContour.ContourSet.Add(point);
                    }
                    else if (drawingColor.ToArgb() != -1)
                    {
                        _error = true;
                    }
                }
            }

        //    var a = Pollen.KoniczynaC.Color.GetDistance(Color.White);  no chyba nie
            if (_error)
            {
               Log.Log("Podczas wczytywania bitmapy program napotkał piksele których nie mógł rozpoznać");
            }
            wynikContour.Bitmap = contourBitmap;
            return wynikContour;
        }
    }
}