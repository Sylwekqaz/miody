﻿using System.Drawing;
using System.Threading.Tasks;
using APP.Model;
using System.IO;
using System;

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
            string fileName = string.Format("errorlog-{0:yyyy-MM-dd_HH-mm-ss}.txt", DateTime.Now);
            TextWriter tw = new StreamWriter(fileName);

            tw.WriteLine("Następujące piksele zawierają nieprawidłowe kolory: ");

            Bitmap contourBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            contourBitmap.MakeTransparent();
            contourBitmap.Clear(Color.Transparent);

            Contour wynikContour = new Contour(bitmap.Width, bitmap.Height);
            int bh = bitmap.Height;
            int bw = bitmap.Width;
            Parallel.For(0, bh, i => { 
            //for (int i = 0; i < bitmap.Height; i++)
            //{
                for (int j = 0; j < bw; j++)
                {
                    Color drawingColor;
                    lock (bitmap)
                    {
                        drawingColor = bitmap.GetPixel(j, i);
                    }

                    var pollen = Pollen.TryPrase(drawingColor.ToMediaColor());
                    if (pollen != null)
                    {
                        ContourPoint point = new ContourPoint
                        {
                            Location = new Point(j, i),
                            Type = pollen
                        };

                        lock (contourBitmap)
                        {
                            contourBitmap.SetPixel(j, i, point.Type.Color.ToDrawingColor());
                        }

                        lock (wynikContour)
                        {
                            wynikContour.ContourSet.Add(point);
                        }
                    }
                    else if (drawingColor.ToArgb() != -1)
                    {
                        lock (tw)
                        {
                            tw.WriteLine("[" + i + "," + j + "]");
                        }
                        _error = true;
                    }
                }
            });

        //    var a = Pollen.KoniczynaC.Color.GetDistance(Color.White);  no chyba nie
            if (_error)
            {
               Log.Log("Podczas wczytywania bitmapy program napotkał piksele których nie mógł rozpoznać");
            }

            tw.Close();

            wynikContour.Bitmap = contourBitmap;
            return wynikContour;
        }
    }
}