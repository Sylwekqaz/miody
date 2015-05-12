using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Model;

namespace APP.Helpers.Measures
{
    class HammingDistance : IComparison
    {
        public Result GetResult(Contour a, Contour b)
        {
            int mocRóżnicyAB = MocRóżnicy(a.ContourSet, b.ContourSet);
            int mocRóżnicyBA = MocRóżnicy(b.ContourSet, a.ContourSet);
            int mocCzęściWspólnej = a.ContourSet.Count - mocRóżnicyAB;

            double wynik = 1 - (mocRóżnicyAB + mocRóżnicyBA) / (a.ContourSet.Count + b.ContourSet.Count - mocCzęściWspólnej);
            return new Result { Title = "Hamming Distance", D = wynik };
        }
        
        private int MocRóżnicy(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            int licznik = 0;

            foreach (ContourPoint item in listaA)
            {
                bool IstniejeRówny = false;
                foreach (ContourPoint item2 in listaB)
                {
                    if (item.Type == item2.Type && item.Location == item2.Location)
                    {
                        IstniejeRówny = true;
                    }
                }
                if (!IstniejeRówny) licznik++;
            }
            return licznik;
        }

    }
}
