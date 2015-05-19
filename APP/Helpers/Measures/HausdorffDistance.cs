using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Model;
using System.Drawing;



namespace APP.Helpers.Measures
{
    

    class HausdorffDistance : IComparison
    {
        public static double BitmapDiagonal(Bitmap bitmap)
        {
            return Math.Sqrt(Math.Pow(bitmap.Height, 2) + Math.Pow(bitmap.Width, 2));


        }

        protected List<double> InfimumList(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            List<double> listaInfimum = new List<double>();


            foreach (ContourPoint item in listaA)
            {
                double minOdleglosc = 100000000;        

                foreach (ContourPoint item2 in listaB)
                {

                    if (item.Type.Numer == item2.Type.Numer)
                    {


                        double odleglosc = item.Location.GetDistance(item2.Location);



                        if (odleglosc < minOdleglosc)
                        {
                            minOdleglosc = odleglosc;
                        }


                    }


                }
                if (minOdleglosc != 100000000)
                {
                    listaInfimum.Add(minOdleglosc);
                }

                //w innym przypadku, tzn jesli w drugim konturze nie ma tego gatunku ktory jest w pierwszym, nie bierzemy w ogole pod uwage tych gatunkow w liczeniu miary
                


            }
            return listaInfimum;
        }


        protected double SupremumList(IList<double> listaInfimum)
        {
            

            double max = listaInfimum[0];
            foreach (double item in listaInfimum)
            {

                if (item>max)
                {
                    max = item;    
                }
                
            }
            return max;
        }

        

        public Result GetResult(Contour a, Contour b)
        {

            List<double> listaInfimumXB = InfimumList(a.ContourSet, b.ContourSet);
            List<double> listaInfimumYA = InfimumList(b.ContourSet, a.ContourSet);


            double pierwszeSupremum = SupremumList(listaInfimumXB);
            double drugieSupremum = SupremumList(listaInfimumYA);

            
            double sup = SupremumList(new List<double>() { pierwszeSupremum, drugieSupremum });
            
            Result obj = new Result();

            obj.Title = "Metryka Hausdorffa";

            obj.D = sup / HausdorffDistance.BitmapDiagonal(a.Bitmap);    //zakladam ze oba kontury maja tych samych rozmiarow bitmape, bo jesli nie, to wedle ktorej bitmapy liczyc przekatna?

            
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
