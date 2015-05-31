using APP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers.FileHandling
{
    class BitmapFromContour
    {
        public static Bitmap Result(Contour contour)
        {
            Bitmap result = new Bitmap(contour.Height, contour.Width);
            foreach (var item in contour.ContourSet)
            {
                result.SetPixel(item.Location.X, item.Location.Y, item.Type.Color.ToDrawingColor());
            }
            return result;

        }
    }
}
