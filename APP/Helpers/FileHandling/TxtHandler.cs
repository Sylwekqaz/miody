using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using APP.Model;

namespace APP.Helpers.FileHandling
{
    public interface ITxtHandler
    {
        Contour LoadTxt(TextReader reader);
    }


    public class TxtHandler : ITxtHandler
    {
        private readonly IErrorLog _log;

        public TxtHandler(IErrorLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Metoda odczytuje z pliku txt dane linijka po linijce
        /// dodając do wynikowego konturu  ContourPoint 
        /// </summary>
        /// <param name="reader">
        /// TextReader od naszego pliku tekstowego
        /// </param>
        /// <returns>
        /// Zwraca kontur
        /// </returns>
        /// Kamil
        public Contour LoadTxt(TextReader reader)
        {
            int w = 0;
            int h = 0;
            string readLine = reader.ReadLine();
            if (readLine==null)
            {
                _log.Log("Plik jest pusty");
                throw new Exception("Plik jest pusty");
            }
            var line = readLine.Split(' ');

            Contour wynikContour;
            try
            {
                w = int.Parse(line[0]);
                h = int.Parse(line[1]);

                wynikContour = new Contour(w + 10, h + 10)
                {
                    Bitmap = new Bitmap(w + 10, h + 10)
                };
            }
            catch (Exception e)
            {
                _log.Log("Linia:1 " + e.Message);
                throw new Exception("Linia:1 " + e.Message, e);
            }


            //parametr is not valid, przyczyna:
            //http://stackoverflow.com/questions/6333681/c-sharp-parameter-is-not-valid-creating-new-bitmap

            IEnumerable<Pollen> pylki = Pollen.Values;

            double podobienstwo = 0;
            int lineNumber = 1;
            while (reader.Peek() != -1)
            {

                lineNumber++;

                try
                {
                    readLine = reader.ReadLine();
                    line = readLine.Split(' ');


                    if (readLine != null)
                    {
                        if (line.Length > 2)
                        {
                            podobienstwo = (double) MatchFinder(pylki, line[2]).ToList<object>()[1];
                            if (podobienstwo > 0.55)
                            {
                                ContourPoint point = new ContourPoint()
                                {
                                    Location = new Point(int.Parse(line[0]), int.Parse(line[1])),
                                    Type = (Pollen) ((string) MatchFinder(pylki, line[2]).ToList<object>()[0])
                                };

                                wynikContour.ContourSet.Add(point);
                                wynikContour.Bitmap.SetPixel(point.Location.X, point.Location.Y,
                                    point.Type.Color.ToDrawingColor());
                            }
                            else
                            {
                                _log.Log("Linia:" + lineNumber + " Nie rozpoznano nazwy pyłku");
                            }
                        }
                        else
                        {
                            _log.Log("Linia:" + lineNumber + " Nie wystaraczająca ilośc danych w wierszu");
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Log("Linia:" + lineNumber + " "+e.Message);
                }
            }
            reader.Close();
            return wynikContour;
        }

        protected IEnumerable<object> MatchFinder(IEnumerable<Pollen> obj, string line)
        {
            double max = 0;
            string podejrzanyTypPylku = " ";

            foreach (Pollen item in obj)
            {
                double odleglosc = item.Name.CompareToString(line);

                if (odleglosc != 1)
                {
                    if (odleglosc > max)
                    {
                        max = odleglosc;
                        podejrzanyTypPylku = item.Name;
                    }
                }
                else
                {
                    max = 1;
                    podejrzanyTypPylku = item.Name;
                    break;
                }
            }

            List<object> result = new List<object>();
            result.Add(podejrzanyTypPylku);
            result.Add(max);
            return result;
        }
    }

    public static class CompareStrings
    {
        public static double CompareToString(this string source, string target)
            //metoda Levenshteina: http://social.technet.microsoft.com/wiki/contents/articles/26805.calculating-percentage-similarity-of-two-strings-in-c.aspx
        {
            if ((source == null) || (target == null)) return 0; //są nieporównywalne czyli 0% podobieństwa
            if ((source.Length == 0) || (target.Length == 0)) return 0; //są nieporównywalne czyli 0% podobieństwa
            if (source == target) return 1; //są identyczne- 100% podobieństwa

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }
            double result = (1.0 -
                             ((double) distance[sourceWordCount, targetWordCount]/
                              (double) Math.Max(source.Length, target.Length)));

            return result;
        }
    }
}