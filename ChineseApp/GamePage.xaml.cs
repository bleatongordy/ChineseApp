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
using ChineseApp.Strokes;
using Microsoft.Xna.Framework;

namespace ChineseApp
{
    public partial class GamePage : PhoneApplicationPage
    {
        private List<List<Vector2>> strokedata;
        private ccData ccChar;
        private ObservableCollection<string> printableData;
        private bool started;

        public GamePage()
        {
            InitializeComponent();
            printableData = new ObservableCollection<string>();
            strokedata = new List<List<Vector2>>();
            started = false;

            StrokeResults.ItemsSource = printableData;
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);
            strokedata.Add(new List<Vector2>());
            strokedata[strokedata.Count - 1].Add(point);
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
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
                CChar c = new CChar(strokedata);
                
                foreach (Vector2 p in c.MidPointList)
                {
                    System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                    l.StrokeStartLineCap = PenLineCap.Round;
                    l.StrokeEndLineCap = PenLineCap.Round;
                    l.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                    l.StrokeThickness = 10;
                    l.X1 = p.X;
                    l.Y1 = p.Y;
                    l.X2 = p.X;
                    l.Y2 = p.Y;

                    DrawArea.Children.Add(l);
                }

                CChar temp = CChar.Parse(ccChar.strokedata);
                foreach (Vector2 p in temp.MidPointList)
                {
                    System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                    l.StrokeStartLineCap = PenLineCap.Round;
                    l.StrokeEndLineCap = PenLineCap.Round;
                    l.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 0));
                    l.StrokeThickness = 10;
                    l.X1 = p.X;
                    l.Y1 = p.Y;
                    l.X2 = p.X;
                    l.Y2 = p.Y;

                    DrawArea.Children.Add(l);
                }

                started = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ViewPage.xaml", UriKind.Relative));
        }
    }
}