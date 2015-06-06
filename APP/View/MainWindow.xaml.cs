using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainControl _mainControl;
        private ContourSelectionControl _contourSelectionControl;
        private ResultControl _resultControl;

        public MainWindow(MainControl mainControl, ContourSelectionControl contourSelectionControl, ResultControl resultControl)
        {
            InitializeComponent();
            MainControl = mainControl;
            ContourSelectionControl = contourSelectionControl;
            ResultControl = resultControl;
        }

        public MainControl MainControl
        {
            get { return _mainControl; }
            set
            {
                _mainControl = value;
                _mainControl.MainWindow = this;
                _mainControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                _mainControl.VerticalAlignment = VerticalAlignment.Stretch;
                MainWindowContainer.Children.Add(_mainControl);
            }
        }

        public ContourSelectionControl ContourSelectionControl
        {
            get { return _contourSelectionControl; }
            set
            {
                _contourSelectionControl = value;
                _contourSelectionControl.MainWindow = this;
                _contourSelectionControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                _contourSelectionControl.VerticalAlignment = VerticalAlignment.Stretch;
                ContourSelectionControlCointainer.Children.Add(_contourSelectionControl);
            }
        }

        public ResultControl ResultControl
        {
            get { return _resultControl; }
            set
            {
                _resultControl = value;
                _resultControl.MainWindow = this;
                _resultControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                _resultControl.VerticalAlignment = VerticalAlignment.Stretch;
                ResultControlCointainer.Children.Add(_resultControl);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            object senderValue = ((Button)sender).Content;
            var viewNumber = Convert.ToInt32(senderValue);
            ChangeView(viewNumber);
        }

        public void ChangeView(int viewNumber)
        {
            this.Resources["TargetGridAnimation"] = ((double) -(viewNumber - 1));
            ((Storyboard)Resources["Storyboard"]).Begin();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ContourSelectionControl.OnClosing(e);
        }
    }

    #region Converters
    public class MyltiplyConverter : MarkupExtension, IValueConverter
    {
        private static MyltiplyConverter _instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new MyltiplyConverter());
        }
    }

    public class LeftMarginConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static LeftMarginConverter _instance;
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter), 0, 0, 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new LeftMarginConverter());
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double multiply = 1;
            foreach (object value in values)
            {
                multiply *= System.Convert.ToDouble(value);
            }
            return new Thickness(multiply, 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    #endregion
}
