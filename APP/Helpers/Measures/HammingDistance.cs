using System.Collections.Generic;
using APP.Model;
using System;

namespace APP.Helpers.Measures
{
    internal class HammingDistance : Comparison
    {
        public HammingDistance()
        {
            Scale = 5;
        }

        public override Result GetResult(Contour a, Contour b)
        {
            double mocRóżnicyAB = MocRóżnicy(a.ContourSet, b.ContourSet);
            double mocRóżnicyBA = MocRóżnicy(b.ContourSet, a.ContourSet);
            double mocCzęściWspólnej = a.ContourSet.Count - mocRóżnicyAB;

            double wynik = (1 -
                           (mocRóżnicyAB + mocRóżnicyBA)/(a.ContourSet.Count + b.ContourSet.Count - mocCzęściWspólnej))*100;
            wynik = Math.Round(wynik, 2);
            return new Result {Title = "Miara 3 (oparta na odległości Hamminga)", D = wynik};
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