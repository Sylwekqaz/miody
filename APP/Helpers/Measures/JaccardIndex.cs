using System.Collections.Generic;
using APP.Model;

namespace APP.Helpers.Measures
{
    internal class JaccardIndex : IComparison
    {
        private readonly MaskGenerator _maskGenerator;

        public JaccardIndex(MaskGenerator maskGenerator)
        {
            _maskGenerator = maskGenerator;
        }

        public Result GetResult(Contour a, Contour b)
        {
            Mask maskaA = _maskGenerator.GenerateMask(a);
            Mask maskaB = _maskGenerator.GenerateMask(b);

            int mocA = MocZbioru(maskaA);
            int mocB = MocZbioru(maskaB);

            int mocCzęściWspólnej = MocCzęściWspólnej(maskaA, maskaB);
            int mocSumy = mocA + mocB - mocCzęściWspólnej;

            double wynik = mocCzęściWspólnej/(double) mocSumy;
            return new Result {Title = "Indeks Jaccarda", D = wynik};
        }

        public int MocZbioru(Mask a)
        {
            IReadOnlyDictionary<Pollen, bool[,]> A = a.MaskMap;

            int licznik = 0;
            foreach (KeyValuePair<Pollen, bool[,]> item1 in A)
            {
                for (int i = 0; i < item1.Value.GetLength(0); i++)
                {
                    for (int j = 0; j < item1.Value.GetLength(1); j++)
                    {
                        if (item1.Value[i, j])
                        {
                            licznik++;
                        }
                    }
                }
            }
            return licznik;
        }

        public int MocCzęściWspólnej(Mask a, Mask b)
        {
            IReadOnlyDictionary<Pollen, bool[,]> A = a.MaskMap;
            IReadOnlyDictionary<Pollen, bool[,]> B = b.MaskMap;
            int licznik = 0;

            foreach (KeyValuePair<Pollen, bool[,]> item1 in A)
            {
                var item2 = B[item1.Key];


                for (int i = 0; i < item1.Value.GetLength(0); i++)
                {
                    for (int j = 0; j < item1.Value.GetLength(1); j++)
                    {
                        if (item1.Value[i, j] && item2[i, j])
                        {
                            licznik++;
                        }
                    }
                }
            }
            return licznik;
        }
    }
}