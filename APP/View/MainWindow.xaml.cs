using APP.Model;
using APP.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APP.Helpers.FileHandling;
using Microsoft.Win32;
using APP.Helpers.Measures;

namespace APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContourLoader _contourLoader;

        private Window ContourSelectionWindow;
        private Window ResultWindow;

        public MainWindow(ContourLoader contourLoader)
        {
            _contourLoader = contourLoader;
            InitializeComponent();

            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
        }

        private void ContourSelectionOpen_Click(object sender, RoutedEventArgs e)
        {            
            if (ContourSelectionWindow == null)
                ContourSelectionWindow = new CounturSelection(new Contour(500,500)); // todo fixme
            ContourSelectionWindow.Show();
        }

        private void ResultOpen_Click(object sender, RoutedEventArgs e)
        {

            if (contour1 != null && contour2 != null)
            {
                /*Result result1 = new Result();
                result1.Title = "Miara1";
                result1.D = Miara1.miara1(null, null);*/

                HausdorffDistance miara2 = new HausdorffDistance();
                Result result2 = miara2.GetResult(contour1, contour2);

                HammingDistance miara3 = new HammingDistance();
                Result result3 = miara3.GetResult(contour1, contour2);

                List<Result> results = new List<Result>();
                results.Add(result2);
                results.Add(result3);

                if (ResultWindow == null)
                    ResultWindow = new ResultWindow(results, null);
                ResultWindow.Show();
            }
            else MessageBox.Show("Wczytaj oba kontury!");
        }


        private Contour contour1 = null;
        private Contour contour2 = null;

        private void LoadContour1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;

            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                contour1 = _contourLoader.LoadContour(openFileDialog1.FileName); 

                Contour1Image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    contour1.Bitmap.GetHbitmap(), 
                    IntPtr.Zero, 
                    System.Windows.Int32Rect.Empty, 
                    BitmapSizeOptions.FromWidthAndHeight((int)contour1.Width, (int)contour1.Height)
                );
            }
        }

        private void ClearContour1_Click(object sender, RoutedEventArgs e)
        {
            contour1 = null;
            Contour1Image.Source = null;
        }

        private void LoadContour2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;

            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                contour2 = _contourLoader.LoadContour(openFileDialog1.FileName);

                Contour2Image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    contour2.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight((int)contour2.Width, (int)contour2.Height)
                );
            }
        }

        private void ClearContour2_Click(object sender, RoutedEventArgs e)
        {
            contour2 = null;
            Contour2Image.Source = null;
        }

    }
}
