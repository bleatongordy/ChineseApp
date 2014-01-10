using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ChineseApp.Resources;
using System.Windows.Media;
using System.ComponentModel;
using ChineseApp.ModelViewNamespace;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Documents;
using ChineseApp.Strokes;
using Microsoft.Xna.Framework;

namespace ChineseApp
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private List<List<Vector2>> strokedata;
        private List<StrokeType> StrokeTypeList;
        private List<Vector2> MidPointList;

        public MainPage()
        {
            InitializeComponent();

            strokedata = new List<List<Vector2>>();
            StrokeTypeList = new List<StrokeType>();
            MidPointList = new List<Vector2>();
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            StartEvent.Text = e.ManipulationOrigin.ToString();
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);
            strokedata.Add(new List<Vector2>());
            strokedata[strokedata.Count - 1].Add(point);
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            MoveEvent.Text = e.ManipulationOrigin.ToString();

            Vector2 oldpoint = strokedata[strokedata.Count - 1].Last();
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);

            if (point.Y >= 0)
            {
                strokedata[strokedata.Count - 1].Add(point);
                Line l = new Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                l.StrokeThickness = 10;
                l.X1 = oldpoint.X;
                l.Y1 = oldpoint.Y;
                l.X2 = point.X;
                l.Y2 = point.Y;
                DrawArea.Children.Add(l);
            }
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            EndEvent.Text = e.ManipulationOrigin.ToString();
            Vector2 oldpoint = strokedata[strokedata.Count - 1].Last();
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);

            if (point.Y >= 0)
            {
                strokedata[strokedata.Count - 1].Add(point);
                Line l = new Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                l.StrokeThickness = 10;
                l.X1 = oldpoint.X;
                l.Y1 = oldpoint.Y;
                l.X2 = point.X;
                l.Y2 = point.Y;
                DrawArea.Children.Add(l);

                /*
                l = new Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.StrokeThickness = 5;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 125, 100, 25));
                int tt = Stroke.determineLine(strokedata[0], 0, new Point(1, 0), 30);
                if (tt > -1)
                {
                    l.X1 = strokedata[0][0].X;
                    l.X2 = strokedata[0][tt].X;
                    l.Y1 = strokedata[0][0].Y;
                    l.Y2 = strokedata[0][tt].Y;
                    DrawArea.Children.Add(l);
                }
                 */
            }
            if (strokedata[strokedata.Count - 1].Count < 2)
                strokedata.RemoveAt(strokedata.Count - 1);

            calcMidpoint(strokedata[strokedata.Count - 1]);
        }

        private void calcMidpoint(List<Vector2> strokedata)
        {
            Vector2 midpoint = new Vector2(0, 0);

            foreach (Vector2 v in strokedata)
            {
                midpoint.X += v.X;
                midpoint.Y += v.Y;
            }
            midpoint.X /= strokedata.Count;
            midpoint.Y /= strokedata.Count;

            MidPointList.Add(midpoint);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ccData c = new ccData();
            c.english = English.Text;
            c.pinyin = Pinyin.Text;
            c.chinese = Chinese.Text;
            c.strokedata = (new CChar(strokedata)).ToString();
            c.id = Chinese.Text[0];
            ModelView.Instance.add(c);
            DrawArea.Children.Clear();
            strokedata.Clear();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StrokeTest.xaml", UriKind.Relative));
        }

        private void English_GotFocus(object sender, RoutedEventArgs e)
        {
            English.Text = "";
        }

        private void Pinyin_GotFocus(object sender, RoutedEventArgs e)
        {
            Pinyin.Text = "";
        }

        private void Chinese_GotFocus(object sender, RoutedEventArgs e)
        {
            Chinese.Text = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}