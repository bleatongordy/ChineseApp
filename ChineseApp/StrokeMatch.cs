using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChineseApp
{
    /*  
     *  The StrokeMatch class compares two StrokeData objects, constructs the different
     *  strokes from their respective parts. It should make data relative, and so therefore
     *  even if the absolute position of strokes are very different, what really matters is
     *  the relative position of the strokes.
    */
    public static class StrokeMatch
    {
        private static double dist(Point p)
        {
            double len = p.X * p.X + p.Y * p.Y;
            return Math.Sqrt(len);
        }

        private static bool checkRelationalData(StrokeData s1, StrokeData s2)
        {
            List<double> propvalue = new List<double>();

            for(int i = 0; i < s1.relationaldata.Count; i++)
            {
                propvalue.Add(dist(s2.relationaldata[i]) / dist(s1.relationaldata[i]));
            }
            return false;
        }

        public static bool compare(StrokeData s1, StrokeData s2)
        {
            // if they have different number of strokes, obviously false
            if (s1.strokedata.Count != s2.strokedata.Count)
                return false;
            return checkRelationalData(s1, s2);
        }
    }
}
