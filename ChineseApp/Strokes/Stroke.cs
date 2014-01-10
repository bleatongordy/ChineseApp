using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChineseApp.Strokes
{
    public enum StrokeType
    {
        ST_INVALID = -1,    // cannot recognize stroke 
        ST_HENG = 0,            // horizontal stroke (written from left to right)
        ST_SHU = 1,             // vertical stroke (written from top to bottom)
        ST_PIE = 2,             // down stroke to the left (written from top right to bottom left)
        ST_NA = 3,              // down stroke to the right (written from top left to bottom right)
        ST_DIAN = 4,            // dot (written from top to bottom right or left)
        ST_TI = 5,              // upward stroke (written from bottom left to top right)
        ST_HENGGOU = 6,         // horizontal stroke with a hook
        ST_SHUGOU = 7,          // vertical stroke with a hook
        ST_WANGGOU = 8,          // bending stroke with a hook
        ST_XIEGOU = 9,          // slant stroke with a hook
        ST_PINGGOU = 10,         // level bending stroke with a hook
        ST_SHUZHE = 11,          // vertical stroke with a horizontal turn to the right
        ST_HENGZHE = 12,         // horizontal stroke with a vertical turn down
        ST_SHUWANGGOU = 13,       // vertical stroke combined with a level bending stroke with a hook
        ST_PIEDIAN = 14,         // down stroke to the left combined with a dot
        ST_SHUZHEZHEGOU = 15     // vertical stroke with a double turn and a hook
    };

    public class Stroke
    {
        public Vector2 Midpoint
        { private set; get; }
        public StrokeType StrokeType
        { private set; get; }

        public Stroke(List<Vector2> strokedata)
        {
            calcMidpoint(strokedata);
            analyze(strokedata);
        }

        public Stroke(List<Vector2> strokedata, StrokeType expectedstroke)
        {
            calcMidpoint(strokedata);
            analyzeExpected(strokedata, expectedstroke);
        }

        private void analyzeExpected(List<Vector2> strokedata, Strokes.StrokeType expectedstroke)
        {
            StrokeType = Strokes.StrokeType.ST_INVALID;
            switch (expectedstroke)
            {
                case Strokes.StrokeType.ST_HENG:
                    if (isHeng(strokedata))
                        StrokeType = Strokes.StrokeType.ST_HENG;
                    break;
                case Strokes.StrokeType.ST_SHU:
                    if (isShu(strokedata))
                        StrokeType = Strokes.StrokeType.ST_SHU;
                    break;
                case Strokes.StrokeType.ST_PIE:
                    if (isPie(strokedata))
                        StrokeType = Strokes.StrokeType.ST_PIE;
                    break;
                case Strokes.StrokeType.ST_NA:
                    if (isNa(strokedata))
                        StrokeType = Strokes.StrokeType.ST_NA;
                    break;
                case Strokes.StrokeType.ST_DIAN:
                    if (isDian(strokedata))
                        StrokeType = Strokes.StrokeType.ST_DIAN;
                    break;
                case Strokes.StrokeType.ST_TI:
                    if (isTi(strokedata))
                        StrokeType = Strokes.StrokeType.ST_TI;
                    break;
                case Strokes.StrokeType.ST_HENGGOU:
                    if (isHengGou(strokedata))
                        StrokeType = Strokes.StrokeType.ST_HENGGOU;
                    break;
                case Strokes.StrokeType.ST_WANGGOU:
                    if (isWangGou(strokedata))
                        StrokeType = Strokes.StrokeType.ST_WANGGOU;
                    break;
                case Strokes.StrokeType.ST_XIEGOU:
                    if (isXieGou(strokedata))
                        StrokeType = Strokes.StrokeType.ST_XIEGOU;
                    break;
                case Strokes.StrokeType.ST_SHUZHE:
                    if (isShuZhe(strokedata))
                        StrokeType = Strokes.StrokeType.ST_SHUZHE;
                    break;
                case Strokes.StrokeType.ST_HENGZHE:
                    if (isHengZhe(strokedata))
                        StrokeType = Strokes.StrokeType.ST_HENGZHE;
                    break;
                case Strokes.StrokeType.ST_SHUWANGGOU:
                    if (isShuWangGou(strokedata))
                        StrokeType = Strokes.StrokeType.ST_SHUWANGGOU;
                    break;
                case Strokes.StrokeType.ST_PIEDIAN:
                    if (isPieDian(strokedata))
                        StrokeType = Strokes.StrokeType.ST_PIEDIAN;
                    break;
                case Strokes.StrokeType.ST_SHUZHEZHEGOU:
                    if (isShuZheZheGou(strokedata))
                        StrokeType = Strokes.StrokeType.ST_SHUZHEZHEGOU;
                    break;
            }
        }

        public static bool isStrokeType(List<Vector2> strokedata, Strokes.StrokeType expectedstroke)
        {
            switch (expectedstroke)
            {
                case Strokes.StrokeType.ST_HENG:
                    return isHeng(strokedata);
                case Strokes.StrokeType.ST_SHU:
                    return isShu(strokedata);
                case Strokes.StrokeType.ST_PIE:
                    return isPie(strokedata);
                case Strokes.StrokeType.ST_NA:
                    return isNa(strokedata);
                case Strokes.StrokeType.ST_DIAN:
                    return isDian(strokedata);
                case Strokes.StrokeType.ST_TI:
                    return isTi(strokedata);
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
                default:
                    return false;
            }
        }

        private void calcMidpoint(List<Vector2> strokedata)
        {
            Vector2 midpoint = new Vector2(0, 0);

            foreach (Vector2 v in strokedata)
            {
                midpoint.X += v.X;
                midpoint.Y += v.Y;
            }
            midpoint.X /= strokedata.Count;
            midpoint.Y /= strokedata.Count;

            Midpoint = midpoint;
        }

        private void analyze(List<Vector2> StrokeData)
        {
            if (isHeng(StrokeData))
                StrokeType = StrokeType.ST_HENG;
            else if (isShu(StrokeData))
                StrokeType = StrokeType.ST_SHU;
            else if (isPie(StrokeData))
                StrokeType = StrokeType.ST_PIE;
            else if (isNa(StrokeData))
                StrokeType = StrokeType.ST_NA;
            else if (isDian(StrokeData))
                StrokeType = StrokeType.ST_DIAN;
            else if (isTi(StrokeData))
                StrokeType = StrokeType.ST_TI;
            else if (isHengGou(StrokeData))
                StrokeType = StrokeType.ST_HENGGOU;
            else if (isShuGou(StrokeData))
                StrokeType = StrokeType.ST_SHUGOU;
            else if (isWangGou(StrokeData))
                StrokeType = StrokeType.ST_WANGGOU;
            else if (isXieGou(StrokeData))
                StrokeType = StrokeType.ST_XIEGOU;
            //            else if (isPingGou(StrokeData))
            //                StrokeType = StrokeType.ST_PINGGOU;
            else if (isShuZhe(StrokeData))
                StrokeType = StrokeType.ST_SHUZHE;
            else if (isHengZhe(StrokeData))
                StrokeType = StrokeType.ST_HENGZHE;
            else if (isShuWangGou(StrokeData))
                StrokeType = StrokeType.ST_SHUWANGGOU;
            else if (isPieDian(StrokeData))
                StrokeType = StrokeType.ST_PIEDIAN;
            else if (isShuZheZheGou(StrokeData))
                StrokeType = StrokeType.ST_SHUZHEZHEGOU;
            else
                StrokeType = StrokeType.ST_INVALID;
        }

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

        // The purpose of a double for loop is to position our center as close as possible
        // to the actual center, and then we keep moving forward until we encompass a whole
        // radius of values, because the initial point will not necessarily be the center
        // of our dot.
        private static int determineDot(List<Vector2> strokedata, int offset, float radius)
        {
            int retvalue = -1;

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

        private static int determineLine(List<Vector2> strokedata, int offset, Vector2 dir, float dof)
        {
            int retvalue = -1;
            float rad = (float)(dof * Math.PI / 180);

            for (int i = offset; i < strokedata.Count - 1; i++)
            {
                if (strokedata[i].Equals(strokedata[i + 1]))
                {
                    if (retvalue > offset)
                        retvalue = i + 1;
                    continue;
                }

                Vector2 dm = new Vector2(strokedata[i + 1].X - strokedata[i].X,
                                         strokedata[i + 1].Y - strokedata[i].Y);

                float angle = (float)Math.Acos(dotprod(dir, dm) / (magnitude(dir) * magnitude(dm)));
                if (angle <= rad)
                    retvalue = i + 1;
                else
                    break;
            }

            return retvalue;
        }

        // Curvature here should represent the amount the slope should be changing in each point
        // the dof (degrees of freedom) should represent the margin of error for that change amount
        private static int determineCurve(List<Vector2> strokedata, int offset, float ds, float dof, bool clockwise)
        {
            int retvalue = -1;
            float dsr = (float)(ds * Math.PI / 180);
            float dofr = (float)(dof * Math.PI / 180);
            Vector2 dm1, dm2, ddm;

            if (offset > strokedata.Count - 2)
                return retvalue;

            dm1 = new Vector2(strokedata[offset + 1].X - strokedata[offset].X,
                            strokedata[offset + 1].Y - strokedata[offset].Y);

            for (int i = offset + 1; i < strokedata.Count - 1; i++)
            {
                if (strokedata[i].Equals(strokedata[i + 1]))
                {
                    if (retvalue > offset)
                        retvalue = i + 1;
                    continue;
                }

                dm2 = new Vector2(strokedata[i + 1].X - strokedata[i].X,
                                strokedata[i + 1].Y - strokedata[i].Y);


                float rad = (float)Math.Atan2(dm1.Y, dm1.X);
                float degree = (float)(rad * 180 / Math.PI);
                ddm = rotate(dm2, rad);
                Vector2 test = rotate(dm1, rad);

                float ddy = (float)Math.Atan2(ddm.Y, ddm.X);
                if (clockwise && ddy < 0)
                    break;

                float angle = (float)Math.Acos(dotprod(dm1, dm2) / (magnitude(dm1) * magnitude(dm2)));

                if (Math.Abs(angle - dsr) <= dofr)
                    retvalue = i + 1;
                else
                    break;

                dm1 = dm2;
            }

            return retvalue;
        }

        private static bool isHeng(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(1, 0), 15);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isShu(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(0, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isPie(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 10);
            if (offset == -1)
                offset = 0;
            int value = determineCurve(strokedata, offset, 5, 10, true);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (strokedata[value].X >= strokedata[0].X || strokedata[value].Y <= strokedata[0].Y)
                return false;
            if (offset == strokedata.Count - 1)
                return true;
            if (value == strokedata.Count - 1)
                return true;

            offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            value = determineLine(strokedata, offset, new Vector2(-1, 1), 25);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isNa(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 10);
            if (offset == -1)
                offset = 0;
            int value = determineCurve(strokedata, offset, 5, 10, false);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (strokedata[value].X <= strokedata[0].X || strokedata[value].Y <= strokedata[0].Y)
                return false;
            if (offset == strokedata.Count - 1)
                return true;
            if (value == strokedata.Count - 1)
                return true;

            offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            value = determineLine(strokedata, offset, new Vector2(1, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isDian(List<Vector2> strokedata)
        {
            int value = determineDot(strokedata, 0, 60);
            if (value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isTi(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(1, -1), 25);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isHengGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(1, 0), 15);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-1, 1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isShuGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(0, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-1, -1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isWangGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineCurve(strokedata, offset, 5, 15, true);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-1, -1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isXieGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineCurve(strokedata, offset, 2, 15, false);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-0.3f, -1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isPingGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineCurve(strokedata, offset, 2, 30, false);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineCurve(strokedata, offset, 2, 30, false);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isShuZhe(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(0, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(1, 0), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isHengZhe(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(1, 0), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(0, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        private static bool isShuWangGou(List<Vector2> strokedata)
        {
            return false;
        }

        public static bool isPieDian(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 5);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(-0.7f, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(0.7f, 1), 20);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 5);
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }

        public static bool isShuZheZheGou(List<Vector2> strokedata)
        {
            int offset = determineDot(strokedata, 0, 10);
            if (offset == -1)
                offset = 0;
            int value = determineLine(strokedata, offset, new Vector2(-0.5f, 1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(1, 0), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-0.3f, 1.0f), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            value = determineLine(strokedata, offset, new Vector2(-1, -1), 30);
            if (value == -1)
                return false;
            offset = determineDot(strokedata, value, 10);
            if (offset == -1)
                offset = value;
            if (offset == strokedata.Count - 1 || value == strokedata.Count - 1)
                return true;
            return false;
        }
    }
}
