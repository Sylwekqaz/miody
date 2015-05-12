using APP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers.FileHandling
{
    public interface IContourLoader
    {
        Contour LoadContour(string path);
    }

    public class ContourLoader // przyjmuje sting path(sciezke do pliku) ->  zwraca kontur.
        // TxtHandler -> przyjmuje plik, jesli jest txt i go zmienia na kontur
        // BitmapHandler -> przyjmuje plik, jesli jest .jpg  i go zmienia na kontur
        : IContourLoader
    {
        private ITxtHandler _txtHandler;
        private IBitmapHandler _bitmapHandler;

        public ContourLoader(IBitmapHandler bitmapHandler, ITxtHandler txtHandler)
        {
            _bitmapHandler = bitmapHandler;
            _txtHandler = txtHandler;
        }
        /// <summary>
        /// Metoda, która sprawdza na podstawie ścieżki do pliku, 
        /// czy mamy doczyniena z plikiem .txt  czy też bitmapą
        /// wywołuje odpowiednie metody w zależności od rozszerzenia pliku
        /// </summary>
        /// <param name="path">
        /// ścieżka do pliku
        /// </param>
        /// <returns>
        /// zwraca kontur
        /// </returns>
        public Contour LoadContour(string path)
        {
           
            Contour loadedContour;

            if (path.EndsWith(".txt"))
            {
                StreamReader reader = new StreamReader(path);
                loadedContour = _txtHandler.LoadTxt(reader);
            }
            else
            {
                Bitmap bitmap = new Bitmap(path);
                loadedContour = _bitmapHandler.LoadBitmap(bitmap);
            }
            return loadedContour;
        }

    }
}
