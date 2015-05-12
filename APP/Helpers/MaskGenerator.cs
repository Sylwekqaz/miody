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


            foreach (ContourPoint contourPoint in contour.ContourSet)
            {
                bitmaps[contourPoint.Type].SetPixel(contourPoint.Location.X,contourPoint.Location.Y,Color.Black);
            }

            //foreach (Pollen pylek in Pollen.Values)
            //{
                bitmaps[/*pylek*/ Pollen.Rzepakowy].FloodFill(new Point(170, 130), 10);


            //}



            return new Mask(height, width);
        }
    }
}