﻿using System;
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

namespace ChineseApp
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private List<List<Point>> strokedata;

        public MainPage()
        {
            InitializeComponent();

            strokedata = new List<List<Point>>(); 
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            StartEvent.Text = e.ManipulationOrigin.ToString();
            Point point = e.ManipulationOrigin;
            strokedata.Add(new List<Point>());
            strokedata[strokedata.Count - 1].Add(point);
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            MoveEvent.Text = e.ManipulationOrigin.ToString();
            
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
            EndEvent.Text = e.ManipulationOrigin.ToString();
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

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ccData c = new ccData();
            c.chapternum = 1; // int.Parse(ChapterNum.Text);
            c.bookname = BookName.Text;
            c.english = English.Text;
            c.pinyin = Pinyin.Text;
            c.chinese = Chinese.Text;
            c.strokedata = new StrokeData(strokedata);
            ModelView.Instance.add(c);
            DrawArea.Children.Clear();
            strokedata.Clear();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
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