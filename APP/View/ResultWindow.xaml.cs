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

namespace APP.View
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private IEnumerable<Comparison> _comparisons;
        private Contour _a;
        private Contour _b;

        private BackgroundWorker _worker;

        public ResultWindow(IEnumerable<Comparison> comparisons, Contour a, Contour b)
        {
            InitializeComponent();
            _comparisons = comparisons;
            _a = a;
            _b = b;

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            _worker.DoWork += Policz;

            _worker.RunWorkerAsync();
        }        

        private void Policz(object sender, DoWorkEventArgs args)
        {
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
                    TextBlockResult.Inlines.Add(""+result.D);

                    if (resultsList.IndexOf(result) != resultsList.Count - 1)
                    {
                        TextBlockTitle.Inlines.Add("\n");
                        TextBlockResult.Inlines.Add("\n");
                    }
                }
                
            }));
        }

        void ComparisonProgresChanged()
        {
            int sumScales = _comparisons.Sum(comparison => comparison.Scale);
            _worker.ReportProgress((int) _comparisons.Sum(comparison => comparison.Progres*comparison.Scale/sumScales));
        }



        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultBar.Visibility = Visibility.Collapsed;            
            
            TextBlock1.Visibility = Visibility.Collapsed;
            TextBlockTitle.Visibility = Visibility.Visible;
            TextBlockResult.Visibility = Visibility.Visible;
        }

        void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ResultBar.Value = e.ProgressPercentage;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}