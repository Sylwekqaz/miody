using System.Drawing;
using System.IO;
using APP.Helpers.FileHandling;
using APP.Model;
using NSubstitute;
using Xunit;
using Point = System.Windows.Point;

namespace Tests
{
    public class ContourLoaderTest
    {
        [Fact]
        public void LoadTxt_ValidData()
        {
            // przygotowanie testu 
            Contour tempContour = new Contour();

            IBitmapHandler bitmapHandler = Substitute.For<IBitmapHandler>();
            bitmapHandler.LoadBitmap(Arg.Any<Bitmap>()).Returns(tempContour);

            ITxtHandler txtHandler = Substitute.For<ITxtHandler>();
            txtHandler.LoadTxt(Arg.Any<TextReader>()).Returns(tempContour);

            string path = @"d:\1.txt";


            ContourLoader contourLoader = new ContourLoader(bitmapHandler,txtHandler);
            

            // wykonanie akcji 
            Contour contour = contourLoader.LoadContour(path);

            //sprawdzenie (asercje)

            Assert.Equal(tempContour,contour);

            txtHandler.Received().LoadTxt(Arg.Any<TextReader>());
            bitmapHandler.DidNotReceive().LoadBitmap(Arg.Any<Bitmap>());

        }
    }
}
