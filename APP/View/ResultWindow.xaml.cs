using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using APP.Helpers.Measures;
using APP.Model;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(IEnumerable<IComparison> comparisons,Contour a , Contour b)
        {
            InitializeComponent();

            TextBlock textBlock = new TextBlock();

            var resultsList = comparisons.Select(comparison => comparison.GetResult(a, b));


            foreach (var result in resultsList)
            {
                textBlock.Inlines.Add(result.Title + ": " + result.D + "\n");
            }

            TextBlock1.Text = textBlock.Text;
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