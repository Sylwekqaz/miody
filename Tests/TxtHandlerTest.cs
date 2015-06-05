using System.Drawing;
using System.IO;
using APP.Helpers;
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
            TxtHandler txtHandler = new TxtHandler(new ErrorLog());

            TextReader reader = new StringReader("10 10\n" +
                                                 "1 1 Rzepak  \n" +
                                                 "1 2 Rzepak  \n" +
                                                 "2 2 Rzepak  \n"); // nie musimy mieć pliku aby przetestować działanie TxtHandler'a

            // wykonanie akcji 
            Contour contour =  txtHandler.LoadTxt(reader);

            //sprawdzenie (asercje)
            Assert.Contains(new ContourPoint { Location = new Point(1, 1), Type = Pollen.NazwyPylkowList["Rzepak"] }, contour.ContourSet);
            Assert.Contains(new ContourPoint { Location = new Point(1, 2), Type = Pollen.NazwyPylkowList["Rzepak"] }, contour.ContourSet);
            Assert.Contains(new ContourPoint { Location = new Point(2, 2), Type = Pollen.NazwyPylkowList["Rzepak"] }, contour.ContourSet);
            Assert.True(contour.ContourSet.Count == 3);

           

        }
    }
}
