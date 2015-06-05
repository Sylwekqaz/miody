using System.Collections.Generic;
using APP.Model;
using System;

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
            if (a.Mask == null)
            {
                a.Mask = _maskGenerator.GenerateMask(a);
            }
            Mask maskaA = a.Mask;
            _maskAComplete = true;
            if (b.Mask == null)
            {
                b.Mask = _maskGenerator.GenerateMask(b);
            }
            Mask maskaB = b.Mask;

            int mocA = MocZbioru(maskaA.MaskMap);
            int mocB = MocZbioru(maskaB.MaskMap);

            int mocCzęściWspólnej = MocCzęściWspólnej(maskaA.MaskMap, maskaB.MaskMap);
            int mocSumy = mocA + mocB - mocCzęściWspólnej;

            double wynik = Math.Round((mocCzęściWspólnej / (double)mocSumy) * 100, 2);

            Progres = 100;
            return new Result {Title = "Miara 1 (oparta na Indeksie Jaccarda)", D = wynik};
            
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