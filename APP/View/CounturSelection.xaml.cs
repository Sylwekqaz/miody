using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls;
using System.IO;
using System.Collections.Generic;

using Microsoft.Win32;

using APP.Model;
using APP.Helpers;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

using Point = System.Windows.Point;
using System.Drawing;
using APP.Helpers.FileHandling;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for CounturSelection.xaml
    /// </summary>
    public partial class CounturSelection : Window
    {
        private IContourSaver _contourSaver;

        private Brush brushColor;
        private Point? currentPoint = null;
        private List<int> przedzial;

        private string saveFileName = "Bitmapa";

        public CounturSelection( Contour a )
        {
            InitializeComponent();

            przedzial = new List<int>();
            przedzial.Add(0);

            IEnumerable<Pollen> values = Pollen.NazwyPylkowList.Values;

            ListColors.ItemsSource = values;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // sprawdzić czy zapisano zmiany
            this.Hide();
        }

        private void LoadContours_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt ";
            openFileDialog1.FilterIndex = 1;

            bool? userClickedOK = openFileDialog1.ShowDialog();
            
            if (userClickedOK == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog1.FileName));

                CanvasContour.Width = bitmapImage.Width;
                CanvasContour.Height = bitmapImage.Height;
                CanvasContourBackground.ImageSource = bitmapImage;                
            }
        }

        private void LoadBackground_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.Filter = "Obrazy (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png";
            openFileDialog1.FilterIndex = 1;
            
            bool? userClickedOK = openFileDialog1.ShowDialog();
            
            if (userClickedOK == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog1.FileName));

                CanvasContour.Width = bitmapImage.Width;
                CanvasContour.Height = bitmapImage.Height;
                CanvasContourBackground.ImageSource = bitmapImage;
            }
        }

        private void TabelaKolorowShow_Click(object sender, RoutedEventArgs e)
        {                  
            if (ListColors.Visibility == Visibility.Collapsed)
                ListColors.Visibility = Visibility.Visible;
            else
                ListColors.Visibility = Visibility.Collapsed;            
        }        

        private void CanvasContour_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (currentPoint != null)
            {
                if (brushColor == null) return;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();

                    line.Stroke = brushColor;
                    line.StrokeThickness = 1;
                    line.X1 = currentPoint.Value.X;
                    line.Y1 = currentPoint.Value.Y;
                    line.X2 = e.GetPosition(CanvasContour).X;
                    line.Y2 = e.GetPosition(CanvasContour).Y;
                    
                    line.HorizontalAlignment = HorizontalAlignment.Left;
                    line.VerticalAlignment = VerticalAlignment.Center;
                    
                    line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

                    currentPoint = e.GetPosition(CanvasContour);
                    
                    CanvasContour.Children.Add(line);
                }
            }

        }

        private void CanvasContour_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(CanvasContour);
        }

        private void CanvasContour_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            currentPoint = null;
            przedzial.Add(CanvasContour.Children.Count);
        }

        private void CanvasContour_MouseLeave(object sender, MouseEventArgs e)
        {
            currentPoint = null;            
        }

        private void CanvasContour_MouseEnter(object sender, MouseEventArgs e)
        {            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(CanvasContour);
            }
        }

        private void SaveContours_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            
            saveFileDialog1.Filter = "Bitmapa (*.bmp)|*.bmp|Plik konturu (*.txt)|*.txt ";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = saveFileName;

            bool? userClickedOK = saveFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                string path = saveFileDialog1.FileName;
                saveFileName = System.IO.Path.GetFileName(path);

                CanvasContourBackground.Opacity = 0;                

                FileStream fs = new FileStream(path, FileMode.Create);
                Rect prostokat = VisualTreeHelper.GetDescendantBounds(CanvasContour);

                RenderTargetBitmap bmp = new RenderTargetBitmap((int)prostokat.Width, (int)prostokat.Height, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual dv = new DrawingVisual();

                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(CanvasContour);
                    dc.DrawRectangle(Brushes.White, null, new Rect(prostokat.Size));                    
                    dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), prostokat.Size));                    
                }

                bmp.Render(dv);

           //     Bitmap bitmap = (Bitmap)bmp;  //to do
               // IContourSaver  ContourSaver;
             //   ContourSaver.SaveContour(path, bitmap);
      
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
                fs.Close();

                CanvasContourBackground.Opacity = 1;
            }
        }

        private void SaveContourAndLoad1_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveContourAndLoad2_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ListViewTypes_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem;
            if (item != null)
            {
                    System.Windows.Media.Color color = (System.Windows.Media.Color)((Pollen)item);
                    System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(color.A,color.R,color.G,color.B);
                    brushColor = new SolidColorBrush(mediaColor);
                }
            }            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();            
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            if (przedzial.Count >= 2) { 
                CanvasContour.Children.RemoveRange(przedzial[przedzial.Count - 2], przedzial[przedzial.Count - 1] - przedzial[przedzial.Count - 2]);
                przedzial.RemoveRange(przedzial.Count - 1, 1);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CanvasContour.Children.Clear();
            przedzial.Clear(); 
            przedzial.Add(0);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                CanvasContourScale.ScaleX *= 1.1;
                CanvasContourScale.ScaleY *= 1.1;                     
                if (CanvasContourScale.ScaleX > 3)
                {
                    CanvasContourScale.ScaleX = 3;
                    CanvasContourScale.ScaleY = 3;
                }
            }
            else
            {                
                CanvasContourScale.ScaleX /= 1.1;
                CanvasContourScale.ScaleY /= 1.1;
                if (CanvasContourScale.ScaleX < 1)
                {
                    CanvasContourScale.ScaleX = 1;
                    CanvasContourScale.ScaleY = 1;
                }
            }
            e.Handled = true;      
        }
    }
}
