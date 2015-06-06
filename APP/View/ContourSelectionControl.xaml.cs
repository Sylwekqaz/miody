﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using APP.Helpers.FileHandling;
using APP.Model;
using Microsoft.Win32;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Path = System.IO.Path;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace APP.View
{
    /// <summary>
    /// Interaction logic for CounturSelection.xaml
    /// </summary>
    public partial class ContourSelectionControl : ContentControl
    {
        private readonly IContourSaver _contourSaver;
        private readonly IBitmapHandler _conveter;
        private readonly IContourLoader _contourLoader;

        private Contour _contour;
        private Brush _brushColor;
        private Point? _currentPoint;
        private readonly List<int> _przedzial;

        private string _saveFileName = "Bitmapa";

        private bool _saveRequired;

        BackgroundWorker worker = new AbortableBackgroundWorker();

        public ContourSelectionControl(IContourSaver contourSaver, IBitmapHandler conveter, IContourLoader contourLoader)
        {
            _contourSaver = contourSaver;
            _conveter = conveter;
            _contourLoader = contourLoader;
            InitializeComponent();

            _przedzial = new List<int> {0};

            IEnumerable<Pollen> values = Pollen.NazwyPylkowList.Values;

            ListColors.ItemsSource = values;

            

        }

        public MainWindow MainWindow { get; set; }


        private void LoadContours_Click(object sender, RoutedEventArgs e)
        {
            if (_saveRequired)
            {
                MessageBoxResult result = MessageBox.Show("Postęp nie został zapisany czy chcesz zapisać?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.None:   // użytkownik pożucił zamykanie okna
                        return;
                        break;
                    case MessageBoxResult.Cancel:  // użytkownik pożucił zamykanie okna
                       return;
                        break;
                    case MessageBoxResult.Yes: //zapisujemy i zamykamy okno
                        SaveContours_Click(null, null);
                        break;

                    case MessageBoxResult.No: //nic nie robimy więc zamyka się okno
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                Contour loadContour;

                try
                {
                    loadContour = _contourLoader.LoadContour(openFileDialog1.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var image  = Imaging.CreateBitmapSourceFromHBitmap(
                    loadContour.Bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(loadContour.Width, loadContour.Height)
                    );

               


                CanvasContour.Children.Clear();
                _przedzial.Clear();
                _przedzial.Add(0);


                // chyba niepotrzebnie zmienia...

                //CanvasContour.Height = image.Height;
                //CanvasContour.Width = image.Width;

                Rectangle rectangle = new Rectangle
                {
                    Fill = new ImageBrush(image),
                    Width = image.Width,
                    Height = image.Height
                };


                rectangle.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Unspecified);

                CanvasContour.Children.Add(rectangle);           
            }
        }

        private void LoadBackground_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Obrazy (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png",
                FilterIndex = 1
            };


            bool? userClickedOk = openFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog1.FileName));

                CanvasContour.Width = bitmapImage.Width;
                CanvasContour.Height = bitmapImage.Height;
                CanvasContourBackground.ImageSource = bitmapImage;
            }
        }

        private void TabelaKolorowShow_Click(object sender, RoutedEventArgs e)
        {
            ListColors.Visibility = ListColors.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void CanvasContour_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentPoint != null)
            {
                if (_brushColor == null) return;
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    Line line = new Line
                    {
                        Stroke = _brushColor,
                        StrokeThickness = 1,
                        X1 = _currentPoint.Value.X,
                        Y1 = _currentPoint.Value.Y,
                        X2 = e.GetPosition(CanvasContour).X,
                        Y2 = e.GetPosition(CanvasContour).Y,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };


                    line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

                    _currentPoint = e.GetPosition(CanvasContour);

                    CanvasContour.Children.Add(line);
                }
            }
        }

        private void CanvasContour_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                _currentPoint = e.GetPosition(CanvasContour);
                if (_brushColor!=null)
                {
                    _saveRequired = true;
                }
                
            }
        }

        private void CanvasContour_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentPoint = null;
            _przedzial.Add(CanvasContour.Children.Count);
        }

        private void CanvasContour_MouseLeave(object sender, MouseEventArgs e) //wolny, bo sie duzo razy wykonuje
        {
            _currentPoint = null;
        }

        private void CanvasContour_MouseEnter(object sender, MouseEventArgs e) //wolny, bo sie duzo razy wykonuje
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _currentPoint = e.GetPosition(CanvasContour);
            }
        }


        private void SavingProcess()
        {
            ResultText.Text = "Trwa zapisywanie...";
            ResultText.Visibility = Visibility.Visible;
        }
        private void SavingProcessEnd()
        {
            ResultText.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Metoda zapisująca Kontur do pliku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// Kamil
        private void SaveContours_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Bitmapa (*.bmp;*.png)|*.bmp;*.png|Plik konturu (*.txt)|*.txt",
                FilterIndex = 1,
                FileName = _saveFileName
            };

            bool? userClickedOk = saveFileDialog1.ShowDialog();

            if (userClickedOk == true)
            {
                
                string path = saveFileDialog1.FileName;
                _saveFileName = Path.GetFileName(path);

                CanvasContourBackground.Opacity = 0;

                Rect prostokat = VisualTreeHelper.GetDescendantBounds(CanvasContour);

                RenderTargetBitmap bmp = new RenderTargetBitmap((int) prostokat.Width, (int) prostokat.Height, 96, 96,
                    PixelFormats.Pbgra32);

                DrawingVisual dv = new DrawingVisual();

                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(CanvasContour);
                    dc.DrawRectangle(Brushes.White, null, new Rect(prostokat.Size));
                    dc.DrawRectangle(vb, null, new Rect(new Point(), prostokat.Size));
                }


                bmp.Render(dv);

                MemoryStream stream = new MemoryStream();
                BitmapEncoder encoder2 = new BmpBitmapEncoder();
                encoder2.Frames.Add(BitmapFrame.Create(bmp));
                encoder2.Save(stream);
                Bitmap bitmap = new Bitmap(stream); 


                worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {

                    System.Windows.Threading.Dispatcher Disp = ResultText.Dispatcher;

                    Disp.Invoke(SavingProcess);

                    _contour = _conveter.LoadBitmap(bitmap);
                    _contourSaver.SaveContour(path, bitmap);

                    _saveRequired = false;

                    Disp.Invoke(SavingProcessEnd);

                };

                worker.RunWorkerAsync();

                CanvasContourBackground.Opacity = 1;
            }
        }

        private void SaveContourAndLoad1_Click(object sender, RoutedEventArgs e)
        {
            SaveContours_Click(null, null);

            MainWindow.MainControl.Contour1 = _contour;
            MainWindow.MainControl.Contour1Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                _contour.Bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(_contour.Width, _contour.Height)
            );
        }

        private void SaveContourAndLoad2_Click(object sender, RoutedEventArgs e)
        {
            SaveContours_Click(null, null);

            MainWindow.MainControl.Contour2 = _contour;
            MainWindow.MainControl.Contour2Image.Source = Imaging.CreateBitmapSourceFromHBitmap(
                _contour.Bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(_contour.Width, _contour.Height)
            );
        }

        private void ListViewTypes_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem;
                if (item != null)
                {
                    Color color = (Color) ((Pollen) item);
                    Color mediaColor = Color.FromArgb(color.A, color.R,
                        color.G, color.B);
                    _brushColor = new SolidColorBrush(mediaColor);
                }
            }
        }

        internal void OnClosing(CancelEventArgs e)
        {
            if (_saveRequired)
            {
                MainWindow.ChangeView(1);
                MessageBoxResult result = MessageBox.Show("Postęp nie został zapisany czy chcesz zapisać?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.None:   // użytkownik pożucił zamykanie okna
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Cancel:  // użytkownik pożucił zamykanie okna
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes: //zapisujemy i zamykamy okno
                        SaveContours_Click(null, null);
                        break;

                    case MessageBoxResult.No: //nic nie robimy więc zamyka się okno
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }  
            }
            



           // e.Cancel = true;
            //Hide();
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_przedzial.Count >= 2)
            {
                CanvasContour.Children.RemoveRange(_przedzial[_przedzial.Count - 2],
                    _przedzial[_przedzial.Count - 1] - _przedzial[_przedzial.Count - 2]);
                _przedzial.RemoveRange(_przedzial.Count - 1, 1);
            }
        }

        // funkcja Wyczyść tło
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CanvasContourBackground.ImageSource = null;
        }

        // funkcja Wyczyść kontury
        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            CanvasContour.Children.Clear();
            _przedzial.Clear();
            _przedzial.Add(0);
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


        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeView(2);
        }
    }
}