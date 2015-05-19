using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Model;

namespace APP.Helpers.Measures
{
    internal class JaccardIndex : IComparison
    {
        public Result GetResult(Contour a, Contour b)
        {
            MaskGenerator Z = new MaskGenerator();
            Mask MaskaA = Z.GenerateMask(a);
            Mask MaskaB = Z.GenerateMask(b);
            int mocA = MocZbioru(MaskaA);
            int mocB = MocZbioru(MaskaB);
            int mocCzęściWspólnej = MocCzęściWspólnej(MaskaA, MaskaB);
            int mocSumy = mocA + mocB - mocCzęściWspólnej;
            double wynik = mocCzęściWspólnej/mocSumy;
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
                        if (item1.Value[i, j] == true)
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
                foreach (KeyValuePair<Pollen, bool[,]> item2 in B)
                {
                    if (item1.Key == item2.Key)
                    {
                        for (int i = 0; i < item1.Value.GetLength(0); i++)
                        {
                            for (int j = 0; j < item1.Value.GetLength(1); j++)
                            {
                                if (item1.Value[i, j] == item2.Value[i, j] == true)
                                {
                                    licznik++;
                                }
                            }
                        }
                    }
                }
            }
            return licznik;
        }
    }
}