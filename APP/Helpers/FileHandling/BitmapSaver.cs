using System.Drawing;

namespace APP.Helpers.FileHandling
{
    public interface IBitmapSaver
    {
        void SaveBitmap(Bitmap bitmap, string fileName);
    }

    internal class BitmapSaver : IBitmapSaver
    {
        /// <summary>
        /// Metoda zapisuje bitmape do pliku
        /// </summary>
        /// <param name="bitmap">
        /// Bitmapa, którą chcemy zapisać 
        /// </param>
        /// <param name="fileName">
        /// Nazwa pliku pod którą chcemy zapisać daną bitmapę
        /// </param>
        /// Kamil
        public void SaveBitmap(Bitmap bitmap, string fileName)
        {
            bitmap.Save(fileName); 
        }
    }
}