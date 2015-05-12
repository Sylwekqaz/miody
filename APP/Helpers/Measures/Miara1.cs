using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers.Measures
{
    class Miara1 
    {
        //todo oganrąć ten burdel tutaj

            static double miara1(int[,] tab1, int[,] tab2)
            {
                int iloczyn = 0;
                int suma = 0; 
                double Result = iloczyn / suma; // todo fixme please 

                for (int i = 0; i < tab1.GetLength(0); i++)
                {
                    for (int j = 0; j < tab1.GetLength(1); j++)
                    {
                        if (tab1[i, j] != 0 && tab1[i, j] == tab2[i, j])
                        {
                            iloczyn++;
                        }
                        if ((tab1[i, j] != 0 && tab2[i, j] == 0) || (tab1[i, j] == 0 && tab2[i, j] != 0))
                        {
                            suma++;
                        }
                        if (tab1[i, j] != 0 && tab2[i, j] != 0 && tab1[i, j] != tab2[i, j])
                        {
                            suma += 2;
                        }
                    }
                }
                //suma = suma - iloczyn;
                return Result;
            }








            
        }
    }


