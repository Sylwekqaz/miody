using System.Collections.Generic;
using System.Linq;

namespace APP.Model
{
    /// <summary>
    /// Klasa modelująca kontury wypełnione
    /// </summary>
    /// /copyright Sylwester Turski
    public class Mask
    {
        public Mask(int height, int width)
        {
            Height = height;
            Width = width;
            var tempMaskMap = Pollen.Values.ToDictionary(value => value, value => new bool[height, width]);
            MaskMap = tempMaskMap;
        }

        //!szerokość rysunku z konturami
        public int Width { get; private set; }
        //!wysokość rysunku z konturami
        public int Height { get; private set; }
        //!słownik tablic boolowskich reprezentujący wypełnione kontury dla poszczególnych gatunków pyłków
        public IReadOnlyDictionary<Pollen, bool[,]> MaskMap { get; private set; }
    }
}