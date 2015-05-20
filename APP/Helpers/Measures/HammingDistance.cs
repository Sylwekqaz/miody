using System.Collections.Generic;
using APP.Model;

namespace APP.Helpers.Measures
{
    internal class HammingDistance : IComparison
    {
        public Result GetResult(Contour a, Contour b)
        {
            double mocRóżnicyAB = MocRóżnicy(a.ContourSet, b.ContourSet);
            double mocRóżnicyBA = MocRóżnicy(b.ContourSet, a.ContourSet);
            double mocCzęściWspólnej = a.ContourSet.Count - mocRóżnicyAB;

            double wynik = 1 -
                           (mocRóżnicyAB + mocRóżnicyBA)/(a.ContourSet.Count + b.ContourSet.Count - mocCzęściWspólnej);
            return new Result {Title = "1 - (względna odległość Hamminga)", D = wynik};
        }

        private int MocRóżnicy(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            int licznik = 0;

            foreach (ContourPoint item in listaA)
            {
                bool istniejeRówny = false;
                foreach (ContourPoint item2 in listaB)
                {
                    if (item.Type == item2.Type && item.Location == item2.Location)
                    {
                        istniejeRówny = true;
                    }
                }
                if (!istniejeRówny) licznik++;
            }
            return licznik;
        }
    }
}