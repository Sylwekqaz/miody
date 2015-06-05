using System.Collections.Generic;
using System.Drawing;

namespace APP.Model
{
    public class Contour
    {
        public Contour(int width, int height)
        {
            Width = width;
            Height = height;
            ContourSet = new HashSet<ContourPoint>();
        }


        public int Width { get; set; }
        public int Height { get; set; }
        public HashSet<ContourPoint> ContourSet { get; set; }
        public Bitmap Bitmap { get; set; } // todo zrobić aby kontur miał rozmiar bitmapy

        public Mask Mask { get; set; }
    }
}