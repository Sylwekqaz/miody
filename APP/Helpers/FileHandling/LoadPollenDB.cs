using APP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace APP.Helpers.FileHandling
{
    class LoadPollenDB
    {

        public static void Load_DB(TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                string readLine = reader.ReadLine();
                string[] line = readLine.Split(' ');
                if (readLine != null)
                {
                    line = readLine.Split(' ');
                    string name = line[0];
                    Color color = (Color) ColorConverter.ConvertFromString(line[1]);
                    // vartable[line[0]] = new Pollen(name, color);
                    new Pollen(name, color);
                }
            }
        }
    }
}
