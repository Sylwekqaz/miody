using System;
using System.IO;
using APP.Model;

namespace APP.Helpers.FileHandling
{
    public interface ITxtSaver
    {
        void SaveTxt(Contour kontur, TextWriter writer);
    }

    internal class TxtSaver : ITxtSaver
    {
        /// <summary>
        /// Metoda zapisuje kontur do pliku tekstowego
        /// </summary>
        /// <param name="kontur">
        /// Kontur, który chcemy zapisać do pliku tekstowego
        /// </param>
        /// <param name="writer">
        /// TextWriter umożliwiający zapis do pliku
        /// </param>
        public void SaveTxt(Contour kontur, TextWriter writer)
        {
            foreach (var item in kontur.ContourSet)
            {
                if (item != null)
                {
                    string pylekNazwa = item.Type;
                    var linia = Convert.ToString(item.Location.X) + " " + Convert.ToString(item.Location.Y) + " " +
                                   Convert.ToString(pylekNazwa);
                    writer.WriteLine(linia);
                }
            }
        }
    }
}