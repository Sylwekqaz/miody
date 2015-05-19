using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using APP.Model;

namespace APP.Helpers
{
    public class MaskGenerator
    {
        public Mask GenerateMask(Contour contour)
        {
            int height = contour.Height;
            int width = contour.Width;

            Dictionary<Pollen, Bitmap> bitmaps = new Dictionary<Pollen, Bitmap>();


            foreach (Pollen pylek in Pollen.Values)
            {
                Bitmap bitmap = new Bitmap(width, height);
                bitmap.Clear(Color.White);

                bitmaps.Add(pylek, bitmap);
            }

#if DEBUG
            HashSet<Pollen> debugPollens = new HashSet<Pollen>();
#endif
            foreach (ContourPoint contourPoint in contour.ContourSet)
            {
                bitmaps[contourPoint.Type].SetPixel(contourPoint.Location.X, contourPoint.Location.Y, Color.Black);
#if DEBUG
                debugPollens.Add(contourPoint.Type);
#endif
            }
            Mask mask = new Mask(height, width);

            Parallel.ForEach(Pollen.Values, pollen =>
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


                bitmaps[pollen].FloodFill(SearchCountur(bitmaps[pollen], tempBitmap, gapBitmap), 10);


                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        mask.MaskMap[pollen][h, w] = bitmaps[pollen].GetPixel(w, h).R == 0;
                    }
                }
            });


            return mask;
        }

        private IEnumerable<Point> SearchCountur(Bitmap bitmap, Bitmap tempBitmap, Bitmap blurBitmap) //todo rename
        {
            var minValue = new {point = new Point(0, 0), value = (byte) 0};
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