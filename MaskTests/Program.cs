﻿using System;
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
            // przygotowanie testu 
            IBitmapHandler handler = new BitmapHandler(new ErrorLog());
            Contour contour = handler.LoadBitmap(new Bitmap(@"..\..\Images\kontur 2.bmp"));
            IMaskGenerator generator = new MaskGenerator();




            // wykonanie akcji 
            var a = generator.GenerateMask(contour);


        }
    }
}
