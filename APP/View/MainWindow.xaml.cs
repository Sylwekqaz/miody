using System;
using System.Collections;
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
using System.Windows.Threading;
using APP.Helpers;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IErrorLog _errorLog;
        private readonly ContourLoader _contourLoader;
        private readonly IEnumerable<Comparison> _comparisons;

        private ContourSelection _contourSelectionWindow;
        private ResultWindow _resultWindow;
        private Contour _contour1;
        private Contour _contour2;

        public Contour Contour1
        {
            get { return _contour1; }
            set
            {
                _contour1 = value;
                if (_contour1 != null && _contour2 != null)
                {
                    GetResultButton.IsEnabled = true;
                }
                else
                {
                    GetResultButton.IsEnabled = false;
                }
            }
        }

        public Contour Contour2
        {
            get { return _contour2; }
            set
            {
                _contour2 = value;
                if (_contour1 != null && _contour2 != null)
                {
                    GetResultButton.IsEnabled = true;
                }
                else
                {
                    GetResultButton.IsEnabled = false;
                }
            }
        }

        public MainWindow(ContourLoader contourLoader, IEnumerable<Comparison> comparisons, IErrorLog errorLog)
        {
            _contourLoader = contourLoader;
            _comparisons = comparisons;
            _errorLog = errorLog;

            _errorLog.Changed +=
                () =>
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action) (() => { ErrorBox.ItemsSource = _errorLog.GetLog(); }));


            InitializeComponent();


            //TextReader writer = new StreamReader(@"../../Pollen.cfg"); //poprawic sciezke
            //LoadPollenDB.Load_DB(writer);
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
            if (Contour1 != null && Contour2 != null)
            {
                SetContourSizes();
                _resultWindow =
                    IoC.Resolve<ResultWindow>(new[]
                    {new NamedParameter("a", Contour1), new NamedParameter("b", Contour2)});
                    // new ResultWindow(results));
                _resultWindow.Show();
            }
            else MessageBox.Show("Wczytaj oba kontury!");
        }

        private void LoadContour1_Click(object sender, RoutedEventArgs e) //open Contour1
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
               
                try
                {
                    Contour1 = _contourLoader.LoadContour(openFileDialog1.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Contour1Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                    Contour1.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(Contour1.Width, Contour1.Height)
                    );

                ListBoxContour1.ItemsSource = Contour1.ContourSet;
            }
        }

        private void ClearContour1_Click(object sender, RoutedEventArgs e)
        {
            Contour1 = null;
            Contour1Image.Source = null;
        }

        private void LoadContour2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                try
                {
                    Contour2 = _contourLoader.LoadContour(openFileDialog1.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Contour2Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                    Contour2.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(Contour2.Width, Contour2.Height)
                    );

                ListBoxContour2.ItemsSource = Contour2.ContourSet;
            }
        }

        private void ClearContour2_Click(object sender, RoutedEventArgs e)
        {
            Contour2 = null;
            Contour2Image.Source = null;
        }

        private void SetContourSizes()
        {
            int width = Math.Max(Contour1.Width, Contour2.Width);
            int height = Math.Max(Contour1.Height, Contour2.Height);
            Contour1.Width = width;
            Contour1.Height = height;
            Contour2.Width = width;
            Contour2.Height = height;
        }
    }
}