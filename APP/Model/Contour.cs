using System.Collections.Generic;
using System.Drawing;

namespace APP.Model
{
    public class Contour
    {
        private int _width;
        private int _height;

        public Contour(int width, int height)
        {
            Width = width;
            Height = height;
            ContourSet = new HashSet<ContourPoint>();
        }


        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                Mask = null;
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Mask = null;
            }
        }

        public HashSet<ContourPoint> ContourSet { get; set; }
        public Bitmap Bitmap { get; set; } 

        public Mask Mask { get; set; }
    }
}