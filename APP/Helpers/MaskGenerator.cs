using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using APP.Model;

namespace APP.Helpers
{
    public class MaskGenerator
    {

        private double _progres;
        public double Progres
        {
            get { return _progres; }
            private set
            {
                _progres = value;
                ProgresChanged();
            }
        }

        public Action ProgresChanged { get; set; }




        public Mask GenerateMask(Contour contour)
        {
            _progres = 0;
            int height = contour.Height;
            int width = contour.Width;

            Dictionary<Pollen, Bitmap> bitmaps = new Dictionary<Pollen, Bitmap>();


            foreach (Pollen pylek in Pollen.Values)
            {
                Bitmap bitmap = new Bitmap(width, height);
                bitmap.Clear(Color.White);

                bitmaps.Add(pylek, bitmap);
            }


            HashSet<Pollen> pollens = new HashSet<Pollen>();
            foreach (ContourPoint contourPoint in contour.ContourSet)
            {
                bitmaps[contourPoint.Type].SetPixel(contourPoint.Location.X, contourPoint.Location.Y, Color.Black);
                pollens.Add(contourPoint.Type);

            }
            Mask mask = new Mask(height, width);

            Parallel.ForEach(pollens, pollen =>
            {
                Bitmap tempBitmap = (Bitmap) bitmaps[pollen].Clone();
                List<Point> nodes = new List<Point>();
                for (int i = 0; i < tempBitmap.Height; i++)
                {
                    nodes.Add(new Point(0, i));
                    nodes.Add(new Point(tempBitmap.Width - 1, i));
                }

                for (int i = 0; i < tempBitmap.Width; i++)
                {
                    nodes.Add(new Point(i, 0));
                    nodes.Add(new Point(i, tempBitmap.Height - 1));
                }
                Bitmap gapBitmap;
                tempBitmap.FloodFill(nodes, 10, out gapBitmap);


                bitmaps[pollen].FloodFill(SearchCountur(bitmaps[pollen], tempBitmap), 10);


                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        mask.MaskMap[pollen][h, w] = bitmaps[pollen].GetPixel(w, h).R == 0;
                    }
                }

                Progres += ((double) 100/pollens.Count());

            });


            return mask;
        }

        private IEnumerable<Point> SearchCountur(Bitmap bitmap, Bitmap tempBitmap) //todo rename
        {
            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    if (tempBitmap.GetPixel(w, h).R != 0) // true dla pixeli w środku konturu
                    {
                        if (bitmap.GetPixel(w, h).R != 0) // true dla pixeli jeszcze nie zamalowanych 
                        {
                            yield return new Point(w, h);
                        }
                    }
                }
            }
        }
    }
}