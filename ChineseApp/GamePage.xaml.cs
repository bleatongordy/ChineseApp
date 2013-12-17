using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Shapes;
using ChineseApp.ModelViewNamespace;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace ChineseApp
{
    public partial class GamePage : PhoneApplicationPage
    {
        private List<List<Point>> strokedata;
        private ccData ccChar;
        private ObservableCollection<string> printableData;
        private bool started;

        public GamePage()
        {
            InitializeComponent();
            printableData = new ObservableCollection<string>();
            strokedata = new List<List<Point>>();
            started = false;

            StrokeResults.ItemsSource = printableData;
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Point point = e.ManipulationOrigin;
            strokedata.Add(new List<Point>());
            strokedata[strokedata.Count - 1].Add(point);
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Point oldpoint = strokedata[strokedata.Count - 1].Last();
            Point point = e.ManipulationOrigin;

            if (point.Y >= 0)
            {
                strokedata[strokedata.Count - 1].Add(point);
                Line l = new Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
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
            Point oldpoint = strokedata[strokedata.Count - 1].Last();
            Point point = e.ManipulationOrigin;
            
            if (point.Y >= 0)
            {
                strokedata[strokedata.Count - 1].Add(point);
                Line l = new Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                l.StrokeThickness = 10;
                l.X1 = oldpoint.X;
                l.Y1 = oldpoint.Y;
                l.X2 = point.X;
                l.Y2 = point.Y;
                DrawArea.Children.Add(l);
            }
            if (strokedata[strokedata.Count - 1].Count < 2)
                strokedata.RemoveAt(strokedata.Count - 1);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ccChar = ModelView.Instance.getRandom();
            EnglishText.Text = ccChar.english;
            ChineseText.Text = ccChar.chinese;
            started = true;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (started)
            {
                StrokeData s = StrokeData.Parse(ccChar.strokedata);

                printableData.Add(StrokeMatch.compare(s, new StrokeData(strokedata)).ToString());

                foreach (List<Point> p in s.strokedata)
                {
                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                        l.StrokeStartLineCap = PenLineCap.Round;
                        l.StrokeEndLineCap = PenLineCap.Round;
                        l.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                        l.StrokeThickness = 10;
                        l.X1 = p[i].X;
                        l.Y1 = p[i].Y;
                        l.X2 = p[i + 1].X;
                        l.Y2 = p[i + 1].Y;

                        DrawArea.Children.Add(l);
                    }
                }
                started = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}