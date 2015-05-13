using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Model;



namespace APP.Helpers.Miary
{
    

    class HausdorffDistance : IComparasion
    {
        protected List<double> InfimumList(HashSet<ContourPoint> listaA, HashSet<ContourPoint> listaB)
        {
            List<double> listaInfimum = new List<double>();
            

            foreach (ContourPoint item in listaA)
            {
                double minOdleglosc = Math.Sqrt(Math.Pow((item.Location.X - listaB.ToList()[0].Location.X), 2) + Math.Pow((item.Location.Y - listaB.ToList()[0].Location.Y), 2)); 

                foreach (ContourPoint item2 in listaB)
                {
                    
                    double odleglosc = Math.Sqrt(Math.Pow((item.Location.X - item2.Location.X), 2) + Math.Pow((item.Location.Y - item2.Location.Y), 2));

                    if (odleglosc < minOdleglosc)
                    {
                        minOdleglosc = odleglosc;
                    }

                }
                listaInfimum.Add(minOdleglosc);

            }
            return listaInfimum;
        }


        protected double SupremumList(List<double> listaInfimum)
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

            Result obj = new Result();

            obj.Title = "Metryka Hausdorffa";

            
            if (pierwszeSupremum>=drugieSupremum)
            {
                obj.D = pierwszeSupremum;
            }
            else
            {
                obj.D = drugieSupremum;
            }

            return obj;



    //mała legenda:
            //zbior a.Contourset= {x1,x2,...,xn}, zbior b.ContourSet= {y1,y2,...,yn}, gdzie xi, czy yi to punkty skladajace sie z dwoch wspolrzednych
            ////listaInfimumXB jest postaci: { inf{d(x1,y1),...,d(x1,yn)}, inf{d(x2,y1),...,d(x2,yn)}, ..., inf{d(xn,y1),...,d(xn,yn)} }
            //listaInfimumYA jest postaci: { inf{d(y1,x1),...,d(y1,xn)}, inf{d(y2,x1),...,d(y2,xn)},...,inf{d(yn,x1),...,d(yn,xn)} }
            //pierwszeSupremum= sup{listaInfimumXB}, drugieSupremum= sup{listaInfimumYA}. 
            //obj.D= sup{pierwszeSupremum, drugieSupremum}



        }
    }

}
