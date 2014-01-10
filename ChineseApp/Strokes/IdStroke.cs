using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseApp.Strokes
{
        // The Purpose of this class is to identify a stroke based on a 
        // series of points. After multiple ways of trying this is another
        // attempt at trying to create a new identification
    public static class IdStroke
    {
        private static double magnitude(Vector2 p)
        {
            double len = p.X * p.X + p.Y * p.Y;
            return Math.Sqrt(len);
        }

        private static double dotprod(Vector2 p1, Vector2 p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        private static double distance(Vector2 p1, Vector2 p2)
        {
            double t = Math.Sqrt((p2.Y - p1.Y) * (p2.Y - p1.Y) + (p2.X - p1.X) * (p2.X - p1.X));
            return t;
        }

        private static Vector2 rotate(Vector2 p, float rad)
        {
            double x, y;
            x = p.X * Math.Cos(rad) + p.Y * Math.Sin(rad);
            y = p.X * -Math.Sin(rad) + p.Y * Math.Cos(rad);
            return new Vector2((float)x, (float)y);
        }

        public static int calcDot(List<Vector2> strokedata, int offset)
        {
            int retvalue = -1;

            float radius = 60;

            if (offset >= strokedata.Count - 1)
                return retvalue;

            Vector2 center = strokedata[offset];

            // want to determine a midpoint
            for (int i = offset + 1; i < strokedata.Count; i++)
            {
                if (distance(center, strokedata[i]) < radius)
                    retvalue = i;
                else if (distance(center, strokedata[i]) >= radius && retvalue != -1)
                {
                    center = strokedata[i - 1];
                    break;
                }
                else
                    return retvalue;
            }

            for (int i = retvalue; i < strokedata.Count; i++)
            {
                if (distance(center, strokedata[i]) <= radius)
                    retvalue = i;
                else
                    break;
            }

            return retvalue;
        }

            // This algorithm uses the first point to create a base and if every other point
            // is within a certain error value we can consider this a Heng stroke
            // should be more efficient than calculating slopes
        public static int calcHeng(List<Vector2> strokedata, int offset)
        {
            const float ERROR = 30;

            int retvalue = offset;

            float ysub = strokedata[offset].Y;
            while (Math.Abs(strokedata[retvalue].Y - ysub) <= ERROR && retvalue < strokedata.Count - 1)
                retvalue++;

                // This makes sure we have a line of correct length
            if (strokedata[retvalue].X - strokedata[offset].X < 30)
                return -1;

            return retvalue;
        }

            // same thing, we create a base x value for this stroke to lie upon
            // if all the strokepoints lie on this base x value then we have a valid
            // stroke
        public static int calcShu(List<Vector2> strokedata, int offset)
        {
            const int ERROR = 30;

            int retvalue = offset;

            float xsub = strokedata[offset].X;
            while (Math.Abs(strokedata[retvalue].X - xsub) <= ERROR && retvalue < strokedata.Count - 1)
                retvalue++;

            // This makes sure we have a line of correct length
            if (strokedata[retvalue].Y - strokedata[offset].Y < 30)
                return -1;

            return retvalue;
        }

            // 42 degrees from the horizontal line is our estimated Ti line
        public static int calcTi(List<Vector2> strokedata, int offset)
        {
            const int ERROR = 30;
            const double degree = 42 * Math.PI / 180;
            float slope = (float) -(Math.Sin(degree) / Math.Cos(degree));

            int retvalue = offset;

            Vector2 origin = strokedata[offset];

            while (true)
            {
                if (retvalue > strokedata.Count)
                    break;

                float x = strokedata[retvalue].X - origin.X;
                float y = x * slope;
                
                if(Math.Abs(strokedata[retvalue].Y - origin.Y - y) >= ERROR)
                    break;
                retvalue++;
            }

            if (strokedata[retvalue].X - strokedata[offset].X < 30)
                return -1;

            return retvalue;
        }

        public static int calcNa(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        public static int calcPie(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        public static int calcZhe(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        public static int calcGou(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        public static int calcWang(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        public static int calcXie(List<Vector2> strokedata, int offset)
        {
            return -1;
        }

        private static bool isHeng(List<Vector2> strokedata)
        {
            if (calcHeng(strokedata, 0) == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isShu(List<Vector2> strokedata)
        {
            if (calcShu(strokedata, 0) == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isTi(List<Vector2> strokedata)
        {
            if (calcTi(strokedata, 0) == strokedata.Count - 1)
                return true;
            return false;
        }

        public static bool isStrokeType(List<Vector2> strokedata, Strokes.StrokeType expectedstroke)
        {
            switch (expectedstroke)
            {
                case Strokes.StrokeType.ST_HENG:
                    return isHeng(strokedata);
                case Strokes.StrokeType.ST_SHU:
                    return isShu(strokedata);
                /*
                case Strokes.StrokeType.ST_PIE:
                    return isPie(strokedata);
                case Strokes.StrokeType.ST_NA:
                    return isNa(strokedata);
                case Strokes.StrokeType.ST_DIAN:
                    return isDian(strokedata);
                */
                case Strokes.StrokeType.ST_TI:
                    return isTi(strokedata);
                /*
                case Strokes.StrokeType.ST_HENGGOU:
                    return isHengGou(strokedata);
                case Strokes.StrokeType.ST_WANGGOU:
                    return isWangGou(strokedata);
                case Strokes.StrokeType.ST_XIEGOU:
                    return isXieGou(strokedata);
                case Strokes.StrokeType.ST_SHUZHE:
                    return isShuZhe(strokedata);
                case Strokes.StrokeType.ST_HENGZHE:
                    return isHengZhe(strokedata);
                case Strokes.StrokeType.ST_SHUWANGGOU:
                    return isShuWangGou(strokedata);
                case Strokes.StrokeType.ST_PIEDIAN:
                    return isPieDian(strokedata);
                case Strokes.StrokeType.ST_SHUZHEZHEGOU:
                    return isShuZheZheGou(strokedata);
                */
                default:
                    return false;
            }
        }
    }
}
