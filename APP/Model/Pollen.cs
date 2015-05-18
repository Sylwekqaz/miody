using System;
using System.Collections.Generic;
using System.Windows.Media;

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


        public static readonly Pollen Kasztan = new Pollen(1, "Kasztanowy", Colors.Blue);

        public static readonly Pollen Rzepak = new Pollen(2, "Rzepakowy", Colors.Crimson);

        public static readonly Pollen Komonica = new Pollen(3, "Komonica", Colors.Cyan);

        public static readonly Pollen Lipa = new Pollen(4, "Lipowy", Colors.DeepPink);

        public static readonly Pollen Akacja = new Pollen(5, "Akacjowy", Colors.Gold);

        public static readonly Pollen Mniszek = new Pollen(6, "Mniszkowy", Colors.Gray);

        public static readonly Pollen Wrzos = new Pollen(7, "Wrzosowy", Colors.Indigo);

        public static readonly Pollen Facelia = new Pollen(8, "Faceliowy", Colors.Coral);

        public static readonly Pollen Malina = new Pollen(9, "Malinowy", Colors.Magenta);

        public static readonly Pollen Wierzba = new Pollen(10, "Wierzbowy", Colors.Lime);

        public static readonly Pollen Nawloc = new Pollen(11, "Nawłociowy", Colors.Navy);

        public static readonly Pollen KoniczynaB = new Pollen(12, "Koniczynowy (biala)", Colors.Orange);

        public static readonly Pollen KoniczynaC = new Pollen(12, "Koniczynowy (czerwona)", Colors.Orange);
        
        public static readonly Pollen Blawatek = new Pollen(13, "Blawatkowy", Colors.Teal);

        public static readonly Pollen Szczaw = new Pollen(14, "Szczawiowy", Colors.Maroon);

        public static readonly Pollen Manuka = new Pollen(15, "Manukowy", Colors.SkyBlue);

        public static readonly Pollen Kapustowa = new Pollen(16, "Kapustowate", Colors.Olive);

        public static readonly Pollen Krwawnik = new Pollen(17, "Krwawnikowy", Colors.DarkSlateGray);
        
        public static readonly Pollen Sliwa = new Pollen(12, "Sliwowy", Colors.YellowGreen);

        public static readonly Pollen Kruszyna = new Pollen(12, "Kruszynowy", Colors.Salmon);
        
        public static readonly Pollen Slonecznik = new Pollen(12, "Slonecznikowy", Colors.Plum);
        
        public static readonly Pollen Ostrozen = new Pollen(12, "Ostrozeniowy", Colors.Red);
        
        public static readonly Pollen Wiaz = new Pollen(12, "Wiazowy", Colors.Sienna)
        
        public static readonly Pollen Wyka = new Pollen(12, "Wykowy", Colors.Black);;
        
        public readonly int Numer;
        public  string Name { get; private set; }
        public Color Color { get; private set; } 



        private Pollen(int numer, string name, Color color)
        {

                this.Numer = numer;
                this.Name = name;
                this.Color = color;
                KolorPylkowList.Add(color, this);
                NazwyPylkowList.Add(name, this);
                NumberList.Add(numer, this);

                _values.Add(this);

        }





        public static Pollen TryPrase(Color color) //zamieniamy  color->pylek
        {
            if (KolorPylkowList.ContainsKey(color))
            {
                return KolorPylkowList[color];
            }
            return null;
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
