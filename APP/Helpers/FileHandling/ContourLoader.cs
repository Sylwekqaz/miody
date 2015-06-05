using System.Drawing;
using System.IO;
using APP.Model;

namespace APP.Helpers.FileHandling
{
    public interface IContourLoader
    {
        Contour LoadContour(string path);
    }
    /// <summary>
    /// przyjmuje string o nazwie path(sciezke do pliku) ->  zwraca kontur.
    // TxtHandler -> przyjmuje plik, jesli jest txt i go zmienia na kontur
    // BitmapHandler -> przyjmuje plik i zmienia go na kontur
    /// </summary>
    /// Kamil
    public class ContourLoader
        : IContourLoader
    {
        private readonly ITxtHandler _txtHandler;
        private readonly IBitmapHandler _bitmapHandler;

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
        /// Kamil
        public Contour LoadContour(string path)
        {
            Contour loadedContour;

            if (Path.GetExtension(path).Contains(".txt"))   //EndsWith(".txt"))
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