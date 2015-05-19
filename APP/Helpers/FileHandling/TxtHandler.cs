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
    public interface ITxtHandler
    {
        Contour LoadTxt(TextReader reader);
    }

    public class TxtHandler : ITxtHandler
    {
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
        public Contour LoadTxt(TextReader reader)
        {
            //Pylek numero = 1;
            //Pylek kolorp = KnownColor.ActiveCaption;
            //Pylek nazwo = "rzepakowy";
            //string name;

            string s = reader.ReadLine();
            if (s == null)
            {
                throw new Exception("pierwsza linijka jest pusta");
            }
            string[] line = s.Split(' ');
            int w = int.Parse(line[0]);
            int h = int.Parse(line[1]);

            Contour wynikContour = new Contour(w, h);

            while (reader.Peek() != -1)
            {
                string readLine = reader.ReadLine();

                //test
                wynikContour.Bitmap = new Bitmap(400, 400);
                //parametr is not valid, przyczyna:
                //http://stackoverflow.com/questions/6333681/c-sharp-parameter-is-not-valid-creating-new-bitmap


                //ogolnie działa np. 
                //for (int i = 0; i < 35; i++)
                //{
                //    for (int j = 0; j < 35; j++)
                //    {
                //        wynikContour.Bitmap.SetPixel(i, j, Color.Red);
                //    }
                //}


                if (readLine != null)
                {
                    line = readLine.Split(' ');
                    ContourPoint point = new ContourPoint()
                    {
                        // kazda linijka to odpowiednio współrzędna: X Y Typ pyłku ;rozna ilosc spacji
                        Location = new Point(int.Parse(line[0]), int.Parse(line[1])),
                        Type = (Pollen) line[2]
                    };
                    wynikContour.ContourSet.Add(point);
                    //wynikContour.Bitmap = new Bitmap(35, 35);
                    wynikContour.Bitmap.SetPixel(point.Location.X, point.Location.Y, point.Type.Color.ToDrawingColor());
                    //wynikContour.Bitmap.SetPixel(point.Location.X, point.Location.Y, Color.Red);
                }
            }


            return wynikContour;
        }
    }
}