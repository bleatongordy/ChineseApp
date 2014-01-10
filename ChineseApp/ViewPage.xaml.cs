using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using ChineseApp.ModelViewNamespace;

namespace ChineseApp
{
    public partial class Page1 : PhoneApplicationPage
    {
        public ObservableCollection<string> printableData;


        public Page1()
        {
            InitializeComponent();

            printableData = new ObservableCollection<string>();
            StrokeResults.ItemsSource = printableData;
            LoadContent();
        }

        public void LoadContent()
        {
            foreach (ccData c in ModelView.Instance.chardata)
            {
                printableData.Add(c.id + " " + c.chinese + " " + c.english + " " + c.pinyin);
                printableData.Add(c.strokedata);
            }
        }
    }
}