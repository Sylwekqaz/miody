using System.Collections.Generic;
using APP.Model;
using System;

namespace APP.Helpers.Measures
{
    /// \copyright Wiktor Suchecki & Krzysztof Traczyk
    internal class HammingDistance : Comparison
    {
        public HammingDistance()
        {
            Scale = 5;
        }
        /// <summary>
        /// Metoda obliczająca miarę podobieństwa opartą na odległości Hamminga. 
        /// Na podstawie dwóch wejściowych obiektów a i b typu Contour, reprezentujących zbiory punktów, zwracana jest wartość równa
        /// (1 -(moc różnicy między a i b + moc różnicy między b i a)/(moc sumy zbiorów a i b))*100. Metoda korzysta z metody prywatnej 
        /// CardinalityOfDifference.
        /// </summary>
        /// <param name="a">
        /// Pierwszy obiekt klasy Contour 
        /// </param>
        /// <param name="b">
        /// Drugi obiekt klasy Contour
        /// </param>
        /// 
        public override Result GetResult(Contour a, Contour b)
        {
            double CountAMinusB = CardinalityOfDifference(a.ContourSet, b.ContourSet);
            double CountBMinusA = CardinalityOfDifference(b.ContourSet, a.ContourSet);
            double CountCommonPart = a.ContourSet.Count - CountAMinusB;

            double result = (1 -
                           (CountAMinusB + CountBMinusA)/(a.ContourSet.Count + b.ContourSet.Count - CountCommonPart))*100;
            result = Math.Round(result, 2);
            return new Result {Title = "Miara 3 (oparta na odległości Hamminga)", D = result};
        }
        /// <summary>
        /// Metoda obliczająca moc różnicy między zbiorem A i zbiorem B
        /// </summary>
        /// <param name="A">
        /// Pierwszy zbiór ContourPointów 
        /// </param>
        /// <param name="B">
        /// Drugi zbiór ContourPointów
        /// </param>
        ///
        private int CardinalityOfDifference(HashSet<ContourPoint> A, HashSet<ContourPoint> B)
        {
            int counter = 0;

            foreach (ContourPoint item in A)
            {
                bool EqualElementExists = false;
                foreach (ContourPoint item2 in B)
                {
                    if (item.Type == item2.Type && item.Location == item2.Location)
                    {
                        EqualElementExists = true;
                    }
                }
                if (!EqualElementExists) counter++;
            }
            return counter;
        }
    }
}