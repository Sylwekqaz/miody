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


        public static readonly Pollen Rzepakowy = new Pollen(1, "Rzepakowy", Colors.Pink);

        public static readonly Pollen Spadziowy = new Pollen(2, "Spadziowy", Colors.Cyan);

        public static readonly Pollen Lipowy = new Pollen(3, "Lipowy", Colors.DeepPink);

        public static readonly Pollen Akacjowy = new Pollen(4, "Akacjowy", Colors.Gold);

        public static readonly Pollen Mniszkowy = new Pollen(5, "Mniszkowy", Colors.Gray);

        public static readonly Pollen Wrzosowy = new Pollen(6, "Wrzosowy", Colors.Indigo);

        public static readonly Pollen Gryczany = new Pollen(7, "Gryczany", Colors.Green);

        public static readonly Pollen Faceliowy = new Pollen(8, "Faceliowy", Colors.Coral);

        public static readonly Pollen Malinowy = new Pollen(9, "Malinowy", Colors.Magenta);

        public static readonly Pollen Nostrzykowy = new Pollen(10, "Nostrzykowy", Colors.Lime);

        public static readonly Pollen Nawłociowy = new Pollen(11, "Nawłociowy", Colors.Navy);

        public static readonly Pollen Koniczynowy = new Pollen(12, "Koniczynowy", Colors.Orange);

        public static readonly Pollen Leśny = new Pollen(13, "Leśny", Colors.SeaGreen);

        public static readonly Pollen Bławatkowy = new Pollen(14, "Bławatkowy", Colors.Teal);

        public static readonly Pollen Cząbrowy = new Pollen(15, "Cząbrowy", Colors.Maroon);

        public static readonly Pollen Manuka = new Pollen(16, "Manuka", Colors.SkyBlue);

        public static readonly Pollen Sadowniczy = new Pollen(17, "Sadowniczy", Colors.Olive);


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