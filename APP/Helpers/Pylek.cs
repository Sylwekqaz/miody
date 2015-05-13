using System.Collections.Generic;
using System.Windows.Media;

namespace APP.Helpers
{
    public sealed class Pylek //nie mozna po  niej dziedziczyc
    {
        
        public static void init()
        {
            Pylek Rzepakowy = new Pylek(1, "Rzepakowy", Colors.Pink);

            Pylek Spadziowy = new Pylek(2, "Spadziowy", Colors.Cyan);

            Pylek Lipowy = new Pylek(3, "Lipowy", Colors.DeepPink);

            Pylek Akacjowy = new Pylek(4, "Akacjowy", Colors.Gold);

            Pylek Mniszkowy = new Pylek(5, "Mniszkowy", Colors.Gray);

            Pylek Wrzosowy = new Pylek(6, "Wrzosowy", Colors.Indigo);

            Pylek Gryczany = new Pylek(7, "Gryczany", Colors.Green);

            Pylek Faceliowy = new Pylek(8, "Faceliowy", Colors.Coral);

            Pylek Malinowy = new Pylek(9, "Malinowy", Colors.Magenta);

            Pylek Nostrzykowy = new Pylek(10, "Nostrzykowy", Colors.Lime);

            Pylek Nawłociowy = new Pylek(11, "Nawłociowy", Colors.Navy);

            Pylek Koniczynowy = new Pylek(12, "Koniczynowy", Colors.Orange);

            Pylek Leśny = new Pylek(13, "Leśny", Colors.SeaGreen);

            Pylek Bławatkowy = new Pylek(14, "Bławatkowy", Colors.Teal);

            Pylek Cząbrowy = new Pylek(15, "Cząbrowy", Colors.Maroon);

            Pylek Manuka = new Pylek(16, "Manuka", Colors.SkyBlue);

            Pylek Sadowniczy = new Pylek(17, "Sadowniczy", Colors.Olive);

        }

        //z czego zmieniamy w co
        //public static readonly SortedList<Color, Pylek> KolorPylkowList = new SortedList<Color, Pylek>();
        public static readonly SortedList<string, Pylek> NazwyPylkowList = new SortedList<string, Pylek>();
        public static readonly SortedList<int, Pylek> NumberList = new SortedList<int, Pylek>();

        public int numer;
        public string name { get; set; }
        public Color color { get; set; }        

        
        public Pylek(int numer, string name, Color color)
        {
            this.numer = numer;
            this.name = name;
            this.color = color;            
            //KolorPylkowList.Add(color, this);
            NazwyPylkowList.Add(name, this);
            NumberList.Add(numer, this);
        }


        /*public static implicit operator Pylek(Color color) //zamieniamy  color->pylek
        {
            //return KolorPylkowList[color];            
        }*/


        public static implicit operator Color(Pylek pylek) //zamieniamy pylek->color
        {
            return pylek.color;
        }


        public static implicit operator Pylek(int numer) //zamieniamy  int->pylek
        {
            return NumberList[numer];
        }

        public static implicit operator int(Pylek pylek) //zamieniamy  pylek->int
        {
            return pylek.numer;
        }

        public static implicit operator Pylek(string name) //zamieniamy  string_name->pylek
        {
            return NazwyPylkowList[name];
        }

        public static implicit operator string(Pylek pylek) //zamieniamy  pylek->string_name
        {
            return pylek.name;
        }

    }
}
