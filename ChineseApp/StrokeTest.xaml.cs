using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ChineseApp.Strokes;

namespace ChineseApp
{
    public partial class StrokeTest : PhoneApplicationPage
    {
        private List<Vector2> strokedata;
        private ObservableCollection<string> StrokeList;

        int m_index;

        const int MAX_INDEX = 15;
        const int MIN_INDEX = 0;

        public StrokeTest()
        {
            InitializeComponent();
            strokedata = new List<Vector2>();
            StrokeList = new ObservableCollection<string>();

            StrokeList.Add("ST_HENG");
            StrokeList.Add("ST_SHU");
            StrokeList.Add("ST_PIE");
            StrokeList.Add("ST_NA");
            StrokeList.Add("ST_DIAN");
            StrokeList.Add("ST_TI");
            StrokeList.Add("ST_HENGGOU");
            StrokeList.Add("ST_SHUGOU");
            StrokeList.Add("ST_WANGGOU");
            StrokeList.Add("ST_XIEGOU");
            StrokeList.Add("ST_PINGGOU");
            StrokeList.Add("ST_SHUZHE");
            StrokeList.Add("ST_HENGZHE");
            StrokeList.Add("ST_SHUWANGGOU");
            StrokeList.Add("ST_PIEDIAN");
            StrokeList.Add("ST_SHUZHEZHEGOU");

            StrokeSelector.ItemsSource = StrokeList;
            m_index = 0;
            StrokeSelector.SelectedItem = StrokeList[m_index];
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);
            strokedata.Clear();
            strokedata.Add(point);
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Vector2 oldpoint = strokedata.Last();
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);

            if (point.Y >= 0)
            {
                strokedata.Add(point);
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
            Vector2 oldpoint = strokedata.Last();
            Vector2 point = new Vector2((float)e.ManipulationOrigin.X, (float)e.ManipulationOrigin.Y);

            if (point.Y >= 0)
            {
                strokedata.Add(point);
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
            if (strokedata.Count < 2)
                strokedata.RemoveAt(strokedata.Count - 1);
        }

        private void StrokeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem.Text = "Selected: " + e.AddedItems[0].ToString();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            bool correct = false;
            StrokeType s = StrokeType.ST_INVALID;
            switch (StrokeSelector.SelectedItem.ToString())
            {
                case "ST_HENG": s = StrokeType.ST_HENG; break;
                case "ST_SHU": s = StrokeType.ST_SHU; break;
                case "ST_PIE": s = StrokeType.ST_PIE; break;
                case "ST_NA": s = StrokeType.ST_NA; break;
                case "ST_DIAN": s = StrokeType.ST_DIAN; break;
                case "ST_TI": s = StrokeType.ST_TI; break;
                case "ST_HENGGOU": s = StrokeType.ST_HENGGOU; break;
                case "ST_SHUGOU": s = StrokeType.ST_SHUGOU; break;
                case "ST_WANGGOU": s = StrokeType.ST_WANGGOU; break;
                case "ST_XIEGOU": s = StrokeType.ST_XIEGOU; break;
                case "ST_PINGGOU": s = StrokeType.ST_PINGGOU; break;
                case "ST_SHUZHE": s = StrokeType.ST_SHUZHE; break;
                case "ST_HENGZHE": s = StrokeType.ST_HENGZHE; break;
                case "ST_SHUWANGGOU": s = StrokeType.ST_SHUWANGGOU; break;
                case "ST_PIEDIAN": s = StrokeType.ST_PIEDIAN; break;
                case "ST_SHUZHEZHEGOU": s = StrokeType.ST_SHUZHEZHEGOU; break;
            }
            correct = IdStroke.isStrokeType(strokedata, s);

            if (correct)
                StrokeOutput.Text = "Correct!";
            else
                StrokeOutput.Text = "Incorrect!";

            List<Vector2> data = new List<Vector2>();
            data.Add(new Vector2(0, 0));
            data.Add(new Vector2(0.33f, 0));
            data.Add(new Vector2(0.66f, 0));
            data.Add(new Vector2(1, 0));
            Matcher m = new Matcher(data);

            StrokeOutput.Text += "\n%Diff: " + m.match(strokedata).ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            DrawArea.Children.Clear();
            strokedata.Clear();
            StrokeOutput.Text = "Press Test";

        }
    }
}