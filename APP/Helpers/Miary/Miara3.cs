using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers.Miary
{
    class Miara3
    {
        //todo oganrąć ten burdel tutaj
        static double miara3(int[,] tab1, int[,] tab2)
        {
            int licznik = 0;
            double Result = licznik / tab1.Length;
            for (int i = 0; i < tab1.GetLength(0); i++)
            {
                for (int j = 0; j < tab1.GetLength(1); j++)
                {
                    if (tab1[i, j] != tab2[i, j])
                    {
                        licznik++;
                    }
                }
            }
            return Result;
        }
    }
}
