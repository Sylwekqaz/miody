using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using APP.Model;

namespace APP.Helpers.Measures
{
    internal class HausdorffDistance : Comparison
    {
        public HausdorffDistance()
        {
            Scale = 5;
        }

        public static double BitmapDiagonal(Bitmap bitmap)
        {
            return Math.Sqrt(Math.Pow(bitmap.Height, 2) + Math.Pow(bitmap.Width, 2));
        }

        protected List<double> InfimumList(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            List<double> listaInfimum = new List<double>();


            foreach (ContourPoint item in listaA)
            {
                double minOdleglosc = double.MaxValue;

                foreach (ContourPoint item2 in listaB)
                {
                    if (item.Type == item2.Type)
                    {
                        double odleglosc = item.Location.GetDistance(item2.Location);


                        if (odleglosc < minOdleglosc)
                        {
                            minOdleglosc = odleglosc;
                        }
                    }
                }
                if (minOdleglosc != double.MaxValue)
                {
                    listaInfimum.Add(minOdleglosc);
                }

                //w innym przypadku, tzn jesli w drugim konturze nie ma tego gatunku ktory jest w pierwszym, nie bierzemy w ogole pod uwage tych gatunkow w liczeniu miary
            }
            return listaInfimum;
        }


        protected double? SupremumList(IList<double> listaInfimum)
        {
            if (listaInfimum.Count == 0)
            {
                return null;
            }
            return listaInfimum.Max();
        }


        public override Result GetResult(Contour a, Contour b)
        {
            List<double> listaInfimumXB = InfimumList(a.ContourSet, b.ContourSet);
            List<double> listaInfimumYA = InfimumList(b.ContourSet, a.ContourSet);


            double? pierwszeSupremum = SupremumList(listaInfimumXB);
            double? drugieSupremum = SupremumList(listaInfimumYA);


            Result obj = new Result();

            if (pierwszeSupremum.HasValue && drugieSupremum.HasValue)
            {
                double sup = Math.Max(pierwszeSupremum.Value, drugieSupremum.Value);
                //zakladam ze oba kontury maja tych samych rozmiarow bitmape, bo jesli nie, to wedle ktorej bitmapy liczyc przekatna?
                obj.D = 1 - sup / BitmapDiagonal(a.Bitmap);
            }
            else
            {
                obj.D = 0;
            }
            obj.Title = "Metryka Hausdorffa";
            
            return obj;


            //mała legenda:
            //zbior a.Contourset= {x1,x2,...,xn}, zbior b.ContourSet= {y1,y2,...,yn}, gdzie xi, czy yi to punkty skladajace sie z dwoch wspolrzednych
            // G1,...,Gm- gatunki, xiGj- i-ty punkt j-tego gatunku z 1 konturu, yiGj- i-ty punkty j-tego gatunku z 2 konturu


            //listaInfimumXB jest postaci:
            /*{ inf{d(x1G1,y1G1),...,d(x1G1,ynG1)},...,inf{d(xnG1,y1G1),...,d(xnG1,ynG1)}, inf{d(x1G2,y1G2),...,d(x1G2,ynG2)},...,inf{d(xnG2,y1G2),...,d(xnG2,ynG2)},
     ,...,inf{d(x1Gm,y1Gm),...,d(x1Gm,ynGm)},...,inf{d(xnGm,y1Gm),...,d(xnGm,ynGm)} }
   

           //listaInfimumYA jest postaci:
  /*
 { inf{d(y1G1,x1G1),...,d(y1G1,xnG1)},...,inf{d(ynG1,x1G1),...,d(ynG1,xnG1)}, inf{d(y1G2,x1G2),...,d(y1G2,xnG2)},...,inf{d(ynG2,x1G2),...,d(ynG2,xnG2)},
     ,...,inf{d(y1Gm,x1Gm),...,d(y1Gm,xnGm)},...,inf{d(ynGm,x1Gm),...,d(ynGm,xnGm)} }
  
      */
            //sup{ sup{listaInfimumXB},sup{listaInfimumYA} }
        }
    }
}