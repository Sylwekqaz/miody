using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using APP.Helpers;

namespace APP.Model
{
    /// <summary>
    /// Klasa która działa jak rozbudowany enum
    /// </summary>
    /// /copyright Kamil
    public sealed class Pollen
    {
        public static readonly Dictionary<Color, Pollen> KolorPylkowList = new Dictionary<Color, Pollen>();
        public static readonly Dictionary<string, Pollen> NazwyPylkowList = new Dictionary<string, Pollen>();
        public static readonly Dictionary<int, Pollen> NumberList = new Dictionary<int, Pollen>();

        private static readonly HashSet<Pollen> _values = new HashSet<Pollen>();

        public static IEnumerable<Pollen> Values
        {
            get { return _values; }
        }

        public readonly int Numer;
        public string Name { get; private set; }
        public Color Color { get; private set; }


        static Pollen()
        {
            TextReader reader = new StreamReader(@"Pollen.cfg");
            while (reader.Peek() != -1)
            {
                string readLine = reader.ReadLine();
                string[] line = readLine.Split(' ');
                if (readLine != null)
                {
                    line = readLine.Split(' ');
                    string name = line[0];
                    Color color = (Color)ColorConverter.ConvertFromString(line[1]);
                    // vartable[line[0]] = new Pollen(name, color);
                    new Pollen(name, color);
                }
            }
        }

        public Pollen(string name, Color color)
        {
            //try
            //{
                Numer = _values.Count + 1;
                Name = name;
                Color = color;
                KolorPylkowList.Add(color, this);
                NazwyPylkowList.Add(name, this);
                NumberList.Add(Numer, this);

                _values.Add(this);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }


        private const double Tolerance = 20;
        /// <summary>
        /// Metoda, która zamienia instancje typu Color w Pollen
        /// </summary>
        /// <param name="color">Parametr typu Color, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji Pollen</returns>
        /// Kamil
        public static Pollen TryPrase(Color color) //zamieniamy  color->pylek
        {
            if (color.A <200)
            {
                return null;
            }
            return Values.FirstOrDefault(pollen => color.GetDistance(pollen.Color) < Tolerance);
        }

        /// <summary>
        /// Metoda, która zamienia instancje typu Pollen w Color
        /// </summary>
        /// <param name="Pollen">Parametr typu Pollen, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji Color</returns>
        /// Kamil
        public static explicit operator Color(Pollen pylek) //zamieniamy pylek->color
        {
            return pylek.Color;
        }

        /// <summary>
        /// Metoda, która zamienia instancje typu int w Pollen
        /// </summary>
        /// <param name="int">Parametr typu int, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji Pollen</returns>
        /// Kamil
        public static implicit operator Pollen(int numer) //zamieniamy  int->pylek
        {
            return NumberList[numer];
        }
        /// <summary>
        /// Metoda, która zamienia instancje typu Pollen w int
        /// </summary>
        /// <param name="Pollen">Parametr typu Pollen, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji int</returns>
        /// Kamil
        public static implicit operator int(Pollen pylek) //zamieniamy  pylek->int
        {
            return pylek.Numer;
        }
        /// <summary>
        /// Metoda, która zamienia instancje typu string w Pollen
        /// </summary>
        /// <param name="string">Parametr typu string, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji Pollen</returns>
        /// Kamil
        public static implicit operator Pollen(string name) //zamieniamy  string_name->pylek
        {
            return NazwyPylkowList[name];
        }
        /// <summary>
        /// Metoda, która zamienia instancje typu Pollen w string
        /// </summary>
        /// <param name="Pollen">Parametr typu Pollen, ten który chcemy zamienić</param>
        /// <returns>Zwraca odpowiendnik instacji string</returns>
        /// Kamil
        public static implicit operator string(Pollen pylek) //zamieniamy  pylek->string_name
        {
            return pylek.Name;
        }
    }
}