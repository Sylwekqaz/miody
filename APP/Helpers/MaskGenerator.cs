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


                bitmaps.Add(pylek, bitmap);

            }

#if DEBUG
             HashSet<Pollen> debugPollens = new HashSet<Pollen>();
#endif
            foreach (ContourPoint contourPoint in contour.ContourSet)
            {
                bitmaps[contourPoint.Type].SetPixel(contourPoint.Location.X,contourPoint.Location.Y,Color.Black);
#if DEBUG
                debugPollens.Add(contourPoint.Type);
#endif
            }

            foreach (Pollen pylek in Pollen.Values)
            {
                bitmaps[pylek].FloodFill(new Point(1, 1), 10);


            }



            return new Mask(height, width);
        }
    }
}