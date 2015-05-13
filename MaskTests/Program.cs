using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Helpers;
using APP.Helpers.FileHandling;
using APP.Model;

namespace MaskTests
{
    class Program
    {
        static void Main(string[] args)
        {
            float asdasd = Color.Black.GetDistance(Color.White);
            float adasdasd = Pollen.Mniszkowy.Color.GetDistance(Color.White);

            // przygotowanie testu 
            IBitmapHandler handler = new BitmapHandler();
            Contour contour = handler.LoadBitmap(new Bitmap(@"..\..\Images\conturUnclosed.png"));
            MaskGenerator generator = new MaskGenerator();




            // wykonanie akcji 
            var a = generator.GenerateMask(contour);


        }
    }
}
