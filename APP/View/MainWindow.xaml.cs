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
            if (ResultWindow == null)
                ResultWindow = new ResultWindow(null, null);
            ResultWindow.Show();
        }
    }
}
