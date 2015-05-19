using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using APP.Helpers;

namespace APP.Model
{
    public sealed class     Pollen  //nie mozna po  niej dziedziczyc
    {

        public static readonly Dictionary<Color, Pollen> KolorPylkowList = new Dictionary<Color, Pollen>();
        public static readonly Dictionary<string, Pollen> NazwyPylkowList = new Dictionary<string, Pollen>();
        public static readonly Dictionary<int, Pollen> NumberList = new Dictionary<int, Pollen>();

        private static readonly HashSet<Pollen> _values = new HashSet<Pollen>();

        public static IEnumerable<Pollen> Values
        {
            get { return _values; }
        }


        public static readonly Pollen Kasztan = new Pollen( "Kasztanowy", Colors.Blue);

        public static readonly Pollen Rzepak = new Pollen( "Rzepakowy", Colors.Crimson);

        public static readonly Pollen Komonica = new Pollen( "Komonica", Colors.Cyan);

        public static readonly Pollen Lipa = new Pollen( "Lipowy", Colors.DeepPink);

        public static readonly Pollen Akacja = new Pollen( "Akacjowy", Colors.Gold);

        public static readonly Pollen Mniszek = new Pollen( "Mniszkowy", Colors.Gray);

        public static readonly Pollen Wrzos = new Pollen( "Wrzosowy", Colors.Indigo);

        public static readonly Pollen Facelia = new Pollen( "Faceliowy", Colors.Coral);

        public static readonly Pollen Malina = new Pollen( "Malinowy", Colors.Magenta);

        public static readonly Pollen Wierzba = new Pollen( "Wierzbowy", Colors.Lime);

        public static readonly Pollen Nawloc = new Pollen( "Nawłociowy", Colors.Navy);

        public static readonly Pollen KoniczynaB = new Pollen( "Koniczynowy (biala)", Colors.Orange);

        public static readonly Pollen KoniczynaC = new Pollen( "Koniczynowy (czerwona)", Colors.Linen);
        
        public static readonly Pollen Blawatek = new Pollen( "Blawatkowy", Colors.Teal);

        public static readonly Pollen Szczaw = new Pollen( "Szczawiowy", Colors.Maroon);

        public static readonly Pollen Manuka = new Pollen( "Manukowy", Colors.SkyBlue);

        public static readonly Pollen Kapustowa = new Pollen( "Kapustowate", Colors.Olive);

        public static readonly Pollen Krwawnik = new Pollen( "Krwawnikowy", Colors.DarkSlateGray);
        
        public static readonly Pollen Sliwa = new Pollen( "Sliwowy", Colors.YellowGreen);

        public static readonly Pollen Kruszyna = new Pollen("Kruszynowy", Colors.Salmon);
        
        public static readonly Pollen Slonecznik = new Pollen("Slonecznikowy", Colors.Plum);
        
        public static readonly Pollen Ostrozen = new Pollen("Ostrozeniowy", Colors.Red);

        public static readonly Pollen Wiaz = new Pollen("Wiazowy", Colors.Sienna);
        
        public static readonly Pollen Wyka = new Pollen( "Wykowy", Colors.Black);
        
        public readonly int Numer;
        public  string Name { get; private set; }
        public Color Color { get; private set; }


        
        private Pollen( string name, Color color)
        {

            try
            {
                this.Numer = _values.Count+1;
                this.Name = name;
                this.Color = color;
                KolorPylkowList.Add(color, this);
                NazwyPylkowList.Add(name, this);
                NumberList.Add(Numer, this);

                _values.Add(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }


        private const float Tolerance = 30F;
        public static Pollen TryPrase(Color color) //zamieniamy  color->pylek
        {
            return Values.FirstOrDefault(pollen => color.GetDistance(pollen.Color) < Tolerance);
        }


        public static explicit operator Color(Pollen pylek) //zamieniamy pylek->color
        {
            return pylek.Color;
        }


        public static implicit operator Pollen(int numer) //zamieniamy  int->pylek
        {
            return NumberList[numer];
        }

        public static implicit operator int(Pollen pylek) //zamieniamy  pylek->int
        {
            return pylek.Numer;
        }

        public static implicit operator Pollen(string name) //zamieniamy  string_name->pylek
        {
            return NazwyPylkowList[name];
        }

        public static implicit operator string(Pollen pylek) //zamieniamy  pylek->string_name
        {
            return pylek.Name;
        }


    }
}
