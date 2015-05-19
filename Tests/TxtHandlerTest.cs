using System.IO;
using System.Windows;
using APP.Helpers.FileHandling;
using APP.Model;
using Xunit;

namespace Tests
{
    public class TxtHandlerTest
    {
        [Fact]
        public void LoadTxt_ValidData()
        {
            // przygotowanie testu 
            TxtHandler txtHandler = new TxtHandler();

            TextReader reader = new StringReader("1 1 1 \n" +
                                                 "1 2 1 \n" +
                                                 "2 2 2 \n"); // nie musimy mieć pliku aby przetestować działanie TxtHandler'a

            // wykonanie akcji 
            Contour contour =  txtHandler.LoadTxt(reader);

            //sprawdzenie (asercje)
            Assert.Contains(new ContourPoint { Location = new Point(1, 1), Type = 1 }, contour.ContourSet);
            Assert.Contains(new ContourPoint { Location = new Point(1, 2), Type = 1 }, contour.ContourSet);
            Assert.Contains(new ContourPoint { Location = new Point(2, 2), Type = 2 }, contour.ContourSet);
            Assert.True(contour.ContourSet.Count == 3);

           

        }
    }
}
