using System.Collections.Generic;
using APP.Model;

namespace APP.Helpers.Measures
{
    class HammingDistance : IComparison
    {
        int _licznik;
        protected HashSet<ContourPoint> ListaWspólnych(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB) //todo refactor name
        {
            HashSet<ContourPoint> listaC = new HashSet<ContourPoint>();

            foreach (ContourPoint item in listaA)
            {
                foreach (ContourPoint item2 in listaB)
                {
                    if (item.Type == item2.Type && item.Location == item2.Location)
                    {
                        listaC.Add(item);
                    }
                    else
                    {
                        _licznik++;
                    }
                }
            }
            return listaC;
        }
        public double Measure(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            _licznik = 0;
            HashSet<ContourPoint> listaC = ListaWspólnych(listaA, listaB);
            double wynik = _licznik / (listaA.Count + listaB.Count - listaC.Count);
            _licznik = 0;
            return wynik;
        }

        public Result GetResult(Contour a, Contour b)
        {
            Result result = new Result
            {
                Title = "Hamming Distance", 
                D = Measure(a.ContourSet, b.ContourSet)
            };

            return result;


        }
    }
}
