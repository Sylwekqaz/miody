using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using APP.Model;
using System.Windows.Controls;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(IEnumerable<Result> resultsList, Bitmap bitmap)
        {
            InitializeComponent();

            TextBlock textBlock = new TextBlock();

            foreach (var result in resultsList)
            {
                textBlock.Inlines.Add(result.Title + ": " + result.D + "\n");
            }

            textBlock1.Text = textBlock.Text;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}