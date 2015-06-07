using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using APP.Helpers.Measures;
using APP.Model;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using APP.Helpers;
using APP.Helpers.FileHandling;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for ResultControl.xaml
    /// </summary>
    public partial class ResultControl : ContentControl
    {
        public MainWindow MainWindow { get; set; }
        private IEnumerable<Comparison> _comparisons;
        private Contour _a;
        private Contour _b;

        private BackgroundWorker _worker;

        private readonly IContourSaver _contourSaver;
        private string _saveFileName = "Bitmapa";
        private string _resultFileString = "";

        public ResultControl()
        {
            InitializeComponent();
        }

        public void GetResult(Contour a, Contour b)
        {
            Clear();

            _a = a;
            _b = b;

            _worker.RunWorkerAsync();
        }


        private void Clear()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;

            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            _worker.DoWork += Policz;


            _comparisons = IoC.Resolve<IEnumerable<Comparison>>();


            ResultBar.Value = 0;

            ResultBar.Visibility = Visibility.Visible;

            TextBlock1.Visibility = Visibility.Visible;
            TextBlockTitle.Visibility = Visibility.Hidden;
            TextBlockResult.Visibility = Visibility.Hidden;

            ResultImage.Visibility = Visibility.Hidden;
            DiffImage.Visibility = Visibility.Hidden;

            ImageSaveButton.IsEnabled = false;
            ResultSaveButton.IsEnabled = false;
            DiffSaveButton.IsEnabled = false;

            BackButton.IsEnabled = false;
        }

        private void Policz(object sender, DoWorkEventArgs args)
        {
            // TextBlock1.Text = "Trwa obliczanie...";  nie bo nalezy do innego wątku..
            var resultsList = new List<Result>();
            foreach (Comparison comparison in _comparisons)
            {
                comparison.ProgresChanged += ComparisonProgresChanged;
                resultsList.Add(comparison.GetResult(_a, _b));
            }


            TextBlock1.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                TextBlockTitle.Inlines.Clear();
                TextBlockResult.Inlines.Clear();

                foreach (var result in resultsList)
                {
                    TextBlockTitle.Inlines.Add(result.Title + ":");
                    TextBlockResult.Inlines.Add("" + result.D + "%");

                    if (resultsList.IndexOf(result) != resultsList.Count - 1)
                    {
                        TextBlockTitle.Inlines.Add("\n");
                        TextBlockResult.Inlines.Add("\n");
                    }

                    _resultFileString += result.Title + ": " + result.D + "%\n";
                }
            }));
        }

        private void ComparisonProgresChanged()
        {
            int sumScales = _comparisons.Sum(comparison => comparison.Scale);
            _worker.ReportProgress((int) _comparisons.Sum(comparison => comparison.Progres*comparison.Scale/sumScales));
        }

        private Bitmap OneOnAnotherBitmap(Bitmap first, Bitmap second)
        {
            //dwie bitmapy moga miec rozne wielkosci, wiec tworzymy nową bitmapę o szerokości=max{bitmapa1,bitmapa2} 
            //i długości=max{bitmapa1,bitmapa2

            int maxWidth = first.Width >= second.Width ? first.Width : second.Width;
            int maxHeight = first.Height >= second.Height ? first.Height : second.Height;


            Bitmap thirst = new Bitmap(maxWidth, maxHeight);
            //wrysujemy do nowej bitmapy pierwsza bitmape; bitmapa1

            Graphics g = Graphics.FromImage(thirst);
            first.MakeTransparent();
            g.DrawImage(first, new System.Drawing.Point(0, 0));

            //tworzymy kolejna nowa bitmape do ktorej przypisujemy 'starą' nową bitmapę

            Bitmap four = thirst;
            g = Graphics.FromImage(four);
            second.MakeTransparent();

            //wrysowujemy do niej drugą bitmapę; bitmapa2

            g.DrawImage(second, new System.Drawing.Point(0, 0));
            return four;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Bitmap res = OneOnAnotherBitmap(_a.Bitmap, _b.Bitmap);

            ResultImage.Source = Imaging.CreateBitmapSourceFromHBitmap(res.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(res.Width, res.Height)
                );

            Bitmap diff = DiffMask(_a.Mask, _b.Mask);//OneOnAnotherBitmap(_a.Bitmap, _b.Bitmap);

            DiffImage.Source = Imaging.CreateBitmapSourceFromHBitmap(diff.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(diff.Width, diff.Height)
                );

            


            ResultBar.Visibility = Visibility.Collapsed;

            TextBlock1.Visibility = Visibility.Collapsed;
            TextBlockTitle.Visibility = Visibility.Visible;
            TextBlockResult.Visibility = Visibility.Visible;

            ResultImage.Visibility = Visibility.Visible;
            DiffImage.Visibility = Visibility.Visible;

            ImageSaveButton.IsEnabled = true;
            ResultSaveButton.IsEnabled = true;
            DiffSaveButton.IsEnabled = true;

            BackButton.IsEnabled = true;




        }


        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ResultBar.Value = e.ProgressPercentage;
        }


        private void DiffSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png",
                FilterIndex = 1,
                FileName = _saveFileName
            };

            bool? userClickedOk = saveFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                string path = saveFileDialog1.FileName;
                Bitmap bitmap = DiffMask(_a.Mask, _b.Mask);

                bitmap.Save(path);
            }
        }

        private void ImageSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png",
                FilterIndex = 1,
                FileName = _saveFileName
            };

            bool? userClickedOk = saveFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                string path = saveFileDialog1.FileName;
                Bitmap bitmap = OneOnAnotherBitmap(_a.Bitmap, _b.Bitmap);

                bitmap.Save(path);
            }
        }

        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Plik tekstowy (*.txt)|*.txt",
                FilterIndex = 1,
                FileName = _saveFileName
            };

            bool? userClickedOk = saveFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                string path = saveFileDialog1.FileName;
                TextWriter writer = new StreamWriter(path);

                writer.WriteLine(_resultFileString);

                writer.Close();
            }
        }

        private Bitmap DiffMask(Mask maskA, Mask maskB)
        {
            Bitmap bitmap = new Bitmap(maskA.Width, maskA.Height);
            Graphics.FromImage(bitmap).Clear(Color.White);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    IEnumerable<Pollen> a = maskA.MaskMap.Where(pair => pair.Value[y,x]).Select(pair => pair.Key);
                    IEnumerable<Pollen> b = maskB.MaskMap.Where(pair => pair.Value[y,x]).Select(pair => pair.Key);
                    if (!a.Any())
                    {
                        if (!b.Any())
                        {
                            continue;
                        }
                        bitmap.SetPixel(x,y,Color.Red);
                    }
                    else
                    {
                        if (!b.Any())
                        {
                            bitmap.SetPixel(x, y, Color.Blue);
                        }
                        else if (a.SequenceEqual(b))
                        {
                            bitmap.SetPixel(x, y, Color.Black);
                        }
                        else
                        {
                            bitmap.SetPixel(x, y, Color.Yellow);
                        }
                    }
                }
            }


            return bitmap;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeView(2);
        }
    }
}