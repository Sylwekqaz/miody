using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using APP.Helpers.FileHandling;
using APP.Helpers.Measures;
using APP.Model;
using Autofac;
using Microsoft.Win32;
using System.IO;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ContourLoader _contourLoader;
        private readonly IEnumerable<Comparison> _comparisons;

        private ContourSelection _contourSelectionWindow;
        private ResultWindow _resultWindow;

        public Contour _contour1;
        public Contour _contour2;

        public MainWindow(ContourLoader contourLoader, IEnumerable<Comparison> comparisons)
        {
            _contourLoader = contourLoader;
            _comparisons = comparisons;
            InitializeComponent();


            TextReader writer = new StreamReader(@"../../../tes.txt"); //poprawic sciezke
            LoadPollenDB.Load_DB(writer);
         // Console.WriteLine(Pollen.Values);  // test
   
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void ContourSelectionOpen_Click(object sender, RoutedEventArgs e)
        {
            _contourSelectionWindow = IoC.Resolve<ContourSelection>();
            _contourSelectionWindow.mainWindow = this;
            _contourSelectionWindow.Show();
        }

        private void ResultOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_contour1 != null && _contour2 != null)
            {
                _resultWindow = IoC.Resolve<ResultWindow>(new[] { new NamedParameter("a", _contour1), new NamedParameter("b", _contour2) });// new ResultWindow(results));
                _resultWindow.Show();
            }
            else MessageBox.Show("Wczytaj oba kontury!");
        }        

        private void LoadContour1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                _contour1 = _contourLoader.LoadContour(openFileDialog1.FileName);
                Contour1Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                    _contour1.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(_contour1.Width, _contour1.Height)
                    );
            }
        }

        private void ClearContour1_Click(object sender, RoutedEventArgs e)
        {
            _contour1 = null;
            Contour1Image.Source = null;
        }

        private void LoadContour2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                _contour2 = _contourLoader.LoadContour(openFileDialog1.FileName);

                Contour2Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                    _contour2.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(_contour2.Width, _contour2.Height)
                    );
            }
        }

        private void ClearContour2_Click(object sender, RoutedEventArgs e)
        {
            _contour2 = null;
            Contour2Image.Source = null;
        }
    }
}