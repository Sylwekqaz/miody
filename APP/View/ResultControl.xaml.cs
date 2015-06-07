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

        private AbortableBackgroundWorker _worker;

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
            if (_worker!=null)
            {
                _worker.Abort();
                _worker.Dispose();
            }

            _worker = new AbortableBackgroundWorker();
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

            ImageSaveButton.IsEnabled = false;
            ResultSaveButton.IsEnabled = false;
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
           // text block z powrotem
            TextBlock1.Text = "Trwa obliczanie..";
            Bitmap res = OneOnAnotherBitmap(_a.Bitmap, _b.Bitmap);

            ResultImage.Source = Imaging.CreateBitmapSourceFromHBitmap(res.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(res.Width, res.Height)
                );

            ResultBar.Visibility = Visibility.Collapsed;

            TextBlock1.Visibility = Visibility.Collapsed;
            TextBlockTitle.Visibility = Visibility.Visible;
            TextBlockResult.Visibility = Visibility.Visible;

            ResultImage.Visibility = Visibility.Visible;

            ImageSaveButton.IsEnabled = true;
            ResultSaveButton.IsEnabled = true;
        }


        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ResultBar.Value = e.ProgressPercentage;
        }


        private void SaveImage_Click(object sender, RoutedEventArgs e)
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

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
            MainWindow.ChangeView(2);
        }
    }

    public class AbortableBackgroundWorker : BackgroundWorker
    {
        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }

        public void Abort()
        {
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread = null;
            }
        }
    }
}