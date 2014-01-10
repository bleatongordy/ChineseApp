using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChineseApp
{
    public struct StrokeCompareData
    {
        public StrokeData s1;
        public StrokeData s2;
        public List<double> positionAnglePercent;
        public List<double> strokeAnglePercent;
        public double distanceVariation;
    }

    /*  
     *  The StrokeMatch class compares two StrokeData objects, constructs the different
     *  strokes from their respective parts. It should make data relative, and so therefore
     *  even if the absolute position of strokes are very different, what really matters is
     *  the relative position of the strokes.
    */
    public static class StrokeMatch
    {
        private static double magnitude(Point p)
        {
            double len = p.X * p.X + p.Y * p.Y;
            return Math.Sqrt(len);
        }

        private static double dotprod(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        private static double calcSlope(Point p1, Point p2)
        {
            double y = p2.Y - p1.Y;
            double x = p2.X - p1.X;

            return y / x;
        }

        private static double variation(List<double> data)
        {
            double mean = 0;
            foreach (double d in data)
                if(!double.IsNaN(d))
                    mean += d;
            mean /= data.Count;
            double var = 0;
            foreach (double d in data)
                if(!double.IsNaN(d))
                    var += (d - mean) * (d - mean);

            return var / data.Count;
        }

        private static void checkStrokeData(StrokeData s1, StrokeData s2)
        {
            List<double> slope1 = new List<double>();
            List<double> slope2 = new List<double>();

            for (int i = 0; i < s1.strokedata[0].Count - 1; i++)
            {
                Point p1 = s1.strokedata[0][i];
                Point p2 = s1.strokedata[0][i + 1];
                slope1.Add(calcSlope(p1, p2));
                p1 = s2.strokedata[0][i];
                p2 = s2.strokedata[0][i + 1];
                slope2.Add(calcSlope(p1, p2));
            }

//            return false;
        }

        private static void checkStrokeDirection(ref StrokeCompareData scd)
        {
            List<double> angles = new List<double>();
            scd.strokeAnglePercent = new List<double>();

            for (int i = 0; i < scd.s1.strokedata.Count; i++)
            {
                Point p1 = new Point(), p2 = new Point();
                p1.X = scd.s1.strokedata[i][scd.s1.strokedata[i].Count - 1].X - scd.s1.strokedata[i][0].X;
                p1.Y = scd.s1.strokedata[i][scd.s1.strokedata[i].Count - 1].Y - scd.s1.strokedata[i][0].Y;
                p2.X = scd.s2.strokedata[i][scd.s2.strokedata[i].Count - 1].X - scd.s2.strokedata[i][0].X;
                p2.Y = scd.s2.strokedata[i][scd.s2.strokedata[i].Count - 1].Y - scd.s2.strokedata[i][0].Y;
                scd.strokeAnglePercent.Add((180 / Math.PI) * Math.Acos(dotprod(p1, p2) / (magnitude(p1) * magnitude(p2))));

//                if (angles[i] > 35)
//                    return false;
            }

//            return true;
        }

        private static void checkRelationalData(ref StrokeCompareData scd)
        {
            // checks distance between strokes
            List<double> propvalue = new List<double>();

            // checks angles between strokes
            List<double> angles = new List<double>();

            scd.positionAnglePercent = new List<double>();

            for(int i = 0; i < scd.s1.relationaldata.Count; i++)
            {
                propvalue.Add(magnitude(scd.s2.relationaldata[i]) / magnitude(scd.s1.relationaldata[i]));
                scd.positionAnglePercent.Add((180 / Math.PI) * Math.Acos(dotprod(scd.s1.relationaldata[i], scd.s2.relationaldata[i]) / (magnitude(scd.s1.relationaldata[i]) * magnitude(scd.s2.relationaldata[i]))));
//                if (propvalue[i] > 5 || angles[i] > 0.87266)
//                    return false;
            }

            scd.distanceVariation = variation(propvalue);

//            return true;
        }

        private static void debugDraw(StrokeData s1, StrokeData s2, Canvas draw)
        {
            for (int i = 0; i < s1.midpointdata.Count; i++)
            {
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                l.StrokeThickness = 3;
                l.X1 = s1.midpointdata[s1.midpointdata.Count / 2].X;
                l.Y1 = s1.midpointdata[s1.midpointdata.Count / 2].Y;
                l.X2 = s1.midpointdata[i].X;
                l.Y2 = s1.midpointdata[i].Y;
                draw.Children.Add(l);
            }

            for (int i = 0; i < s1.midpointdata.Count; i++)
            {
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.StrokeStartLineCap = PenLineCap.Round;
                l.StrokeEndLineCap = PenLineCap.Round;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
                l.StrokeThickness = 3;
                l.X1 = s2.midpointdata[s2.midpointdata.Count / 2].X;
                l.Y1 = s2.midpointdata[s2.midpointdata.Count / 2].Y;
                l.X2 = s2.midpointdata[i].X;
                l.Y2 = s2.midpointdata[i].Y;
                draw.Children.Add(l);
            }
        }

        public static StrokeCompareData compare(StrokeData s1, StrokeData s2)
        {
            // if they have different number of strokes, obviously false
//            if (s1.strokedata.Count != s2.strokedata.Count)
//                return false;

            StrokeCompareData scd = new StrokeCompareData();
            scd.s1 = s1;
            scd.s2 = s2;
            checkRelationalData(ref scd);
            checkStrokeDirection(ref scd);

            return scd;
        }
    }
}
