using System.Collections.Generic;
using APP.Model;

namespace APP.Helpers.Measures
{
    internal class JaccardIndex : Comparison
    {


        private readonly MaskGenerator _maskGenerator;


        void SubProgresChanged()
        {
            Progres = (_maskAComplete ? 40:0) + _maskGenerator.Progres * 0.4;
          
        }

        private bool _maskAComplete;


        public JaccardIndex(MaskGenerator maskGenerator)
        {
            _maskGenerator = maskGenerator;
            _maskGenerator.ProgresChanged = SubProgresChanged;
            Scale = 90;

        }

        public override Result GetResult(Contour a, Contour b)
        {
            _maskAComplete = false;
            _progres = 0;
            
            Mask maskaA = _maskGenerator.GenerateMask(a);
            _maskAComplete = true;
            Mask maskaB = _maskGenerator.GenerateMask(b);

            int mocA = MocZbioru(maskaA.MaskMap);
            int mocB = MocZbioru(maskaB.MaskMap);

            int mocCzęściWspólnej = MocCzęściWspólnej(maskaA.MaskMap, maskaB.MaskMap);
            int mocSumy = mocA + mocB - mocCzęściWspólnej;

            double wynik = mocCzęściWspólnej/(double) mocSumy;

            Progres = 100;
            return new Result {Title = "Indeks Jaccarda", D = wynik};
            
        }



        public int MocZbioru(IReadOnlyDictionary<Pollen, bool[,]> a)
        {
            

            int licznik = 0;
            foreach (KeyValuePair<Pollen, bool[,]> item1 in a)
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

        public int MocCzęściWspólnej(IReadOnlyDictionary<Pollen, bool[,]> a, IReadOnlyDictionary<Pollen, bool[,]> b)
        {
            
            int licznik = 0;

            foreach (KeyValuePair<Pollen, bool[,]> item1 in a)
            {
                var item2 = b[item1.Key];


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