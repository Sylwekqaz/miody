using System.Collections.Generic;
using System.Drawing;

namespace APP.Model
{
    /// <summary>
    /// Klasa modelująca kontury
    /// </summary>
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

        //! szerokość rysunku z konturami
        public int Width 
        {
            get { return _width; }
            set
            {
                _width = value;
                Mask = null;
            }
        }

        //! wysokość rysunku z konturami
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Mask = null;
            }
        }

        public HashSet<ContourPoint> ContourSet { get; set; } //!< zbiór punktów tworzących kontury
        
        public Bitmap Bitmap { get; set; } //!< bitmapa reprezentująca kontury

        public Mask Mask { get; set; } //!< kontury wypełnione
    }
}