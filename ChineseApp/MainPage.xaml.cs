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

namespace ChineseApp
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private ModelView modelview;
        private List<List<Point>> strokedata;

        public MainPage()
        {
            InitializeComponent();

            modelview = new ModelView();
            strokedata = new List<List<Point>>();
        }

        private void Grid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            StartEvent.Text = e.ManipulationOrigin.ToString();
            Point point = e.ManipulationOrigin;

            strokedata.Add(new List<Point>());
            strokedata[strokedata.Count - 1].Add(point);
        }

        private void Grid_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            MoveEvent.Text = e.ManipulationOrigin.ToString();
            Point oldpoint = strokedata[strokedata.Count - 1].Last();
            Point point = e.ManipulationOrigin;

            if (point.Y >= 0)
            {
                strokedata[strokedata.Count - 1].Add(point);

                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                l.StrokeThickness = 10;
                l.X1 = oldpoint.X;
                l.Y1 = oldpoint.Y;
                l.X2 = point.X;
                l.Y2 = point.Y;

                DrawArea.Children.Add(l);
            }
        }

        private void Grid_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            EndEvent.Text = e.ManipulationOrigin.ToString();
            if (strokedata[strokedata.Count - 1].Count < 2)
                strokedata.RemoveAt(strokedata.Count - 1);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ccData c = new ccData();
            c.chapternum = 1; // int.Parse(ChapterNum.Text);
            c.bookname = BookName.Text;
            c.english = English.Text;
            c.pinyin = Pinyin.Text;
            c.chinese = Chinese.Text;
            c.strokedata = new StrokeData(strokedata);
            modelview.add(c);

            DrawArea.Children.Clear();
            strokedata.Clear();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            ccData c = modelview.retrieve(0);

            ChapterNum.Text = c.chapternum.ToString();
            BookName.Text = c.bookname;
            English.Text = c.english;
            Pinyin.Text = c.pinyin;
            Chinese.Text = c.chinese;
            StrokeData s = StrokeData.Parse(c.strokedata);

            StrokeMatch.compare(s, new StrokeData(strokedata));

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
        }

        private void ChapterNum_GotFocus(object sender, RoutedEventArgs e)
        {
            ChapterNum.Text = "";
        }

        private void BookName_GotFocus(object sender, RoutedEventArgs e)
        {
            BookName.Text = "";
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