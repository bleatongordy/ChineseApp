using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ChineseApp
{

    public class ccDB : DataContext
    {
        public ccDB(string connect) : base(connect) { }
        public Table<ccData> data;
    }

    public class StrokeData
    {
        // strokeData will be the list of points for each stroke
        private List<List<Point>> strokeData;
        public List<List<Point>> strokedata
        { get { return strokeData; } }

        // relationalData will be the distance from the midpoints from the first stroke
        // the first point should be (0,0) (the distance from the first stroke to itself)
        private List<Point> relationalData;
        public List<Point> relationaldata
        { get { return relationalData; } }

        private List<Point> midpointData;
        public List<Point> midpointdata
        { get { return midpointData; } }

        public StrokeData(List<List<Point>> strokedata)
        {
            strokeData = strokedata; // wary of this, might need to manually copy elements
            relationalData = new List<Point>();
            midpointData = new List<Point>();
            calcData();
        }

        public static implicit operator string(StrokeData d)
        {
            return d.ToString();
        }

        // Necessary to store into the database as a string
        public override string ToString()
        {
            string srep = "";
            for (int i = 0; i < strokeData.Count; i++)
            {  
                foreach (Point p in strokeData[i])
                {
                    srep += p.ToString() + "|";
                }
                srep += ":";
            }
            return srep;
        }

        // Used to reconstruct the object from the string
        public static StrokeData Parse(string data)
        {
            string[] strokes = data.Split(':');
            List<List<Point>> sdata = new List<List<Point>>();

            for(int j = 0; j < strokes.Count() - 1; j++)
            {
                sdata.Add(new List<Point>());
                string[] points = strokes[j].Split('|');
                for (int i = 0; i < points.Count() - 1; i++) // minus 1 because the last string will be the empty string
                {
                    string[] pdata = points[i].Split(',');
                    Point p = new Point();
                    p.X = double.Parse(pdata[0]);
                    p.Y = double.Parse(pdata[1]);
                    sdata[sdata.Count - 1].Add(p);
                }
            }
            StrokeData ret = new StrokeData(sdata);

            return new StrokeData(sdata);
        }

        private Point calcMidpoint(List<Point> data)
        {
            double x = 0, y = 0;
            for (int i = 0; i < data.Count; i++)
            {
                x += data[i].X;
                y += data[i].Y;
            }
            x /= data.Count;
            y /= data.Count;

            return new Point(x, y);
        }

        private void calcData()
        {
            Point m1, m2;

            if (strokedata.Count < 1)
                return;

            m1 = calcMidpoint(strokeData[strokeData.Count / 2]);

            for (int i = 0; i < strokeData.Count; i++)
            {
                m2 = calcMidpoint(strokeData[i]);
                midpointData.Add(m2);
                relationalData.Add(new Point(m2.X - m1.X, m2.Y - m1.Y));
            }
        }
    }

    [Table]
    public class ccData : INotifyPropertyChanged
    {
        private int _id;
        [Column(IsPrimaryKey = true)]
        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        private string _english;
        [Column]
        public string english
        {
            get { return _english; }
            set
            {
                if (_english != value)
                {
                    _english = value;
                    NotifyPropertyChanged("english");
                }
            }
        }

        private string _pinyin;
        [Column]
        public string pinyin
        {
            get { return _pinyin; }
            set
            {
                if (_pinyin != value)
                {
                    _pinyin = value;
                    NotifyPropertyChanged("pinyin");
                }
            }
        }

        private string _chinese;
        [Column]
        public string chinese
        {
            get { return _chinese; }
            set
            {
                if (_chinese != value)
                {
                    _chinese = value;
                    NotifyPropertyChanged("chinese");
                }
            }
        }

        private string _strokedata;
        [Column]
        public string strokedata
        {
            get { return _strokedata; }
            set
            {
                if (_strokedata != value)
                {
                    _strokedata = value;
                    NotifyPropertyChanged("strokedata");
                }
            }
        }
 
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
}