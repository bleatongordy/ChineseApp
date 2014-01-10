using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseApp.Strokes
{
    public class Matcher
    {
        private List<Vector2> points;
        private int numGroups;

            // these points should be groupings
        public Matcher(List<Vector2> data)
        {
            points = data;
            numGroups = points.Count;
        }
            // the purpose of this function is to scale the data so that the coordinates
            // go from 0,0 to 1,1
            // we ideally want our bounds to be 0,0 to 1,1
        private List<Vector2> scale(List<Vector2> data)
        {
            if (data.Count == 0)
                return null;

            Vector2 upLeftBound = new Vector2();
            Vector2 downRightBound = new Vector2();
            upLeftBound.X = downRightBound.X = data[0].X;
            upLeftBound.Y = downRightBound.Y = data[0].Y;

            foreach (Vector2 v in data)
            {
                if (v.X < upLeftBound.X)
                    upLeftBound.X = v.X;
                if (v.X > downRightBound.X)
                    downRightBound.X = v.X;
                if (v.Y < upLeftBound.Y)
                    upLeftBound.Y = v.Y;
                if (v.Y > downRightBound.Y)
                    downRightBound.Y = v.Y;
            }

            float xoffset = -upLeftBound.X;
            float yoffset = -upLeftBound.Y;
            float dx = 1.0f / (downRightBound.X - upLeftBound.X);
            float dy = 1.0f / (downRightBound.Y - upLeftBound.Y);

            List<Vector2> temp = new List<Vector2>();

            foreach (Vector2 v in data)
            {
                float x = (v.X + xoffset) * dx;
                float y = (v.Y + yoffset) * dy;
                temp.Add(new Vector2(x, y));
            }

            return temp;
        }

        // we want this to return a confidence level
        public float match(List<Vector2> data)
        {
            List<Vector2> groups = createGroups(data);
            return analyze(groups);
        }

        // this function will border our data along the x and y axis
        private List<Vector2> align(List<Vector2> groups)
        {
            List<Vector2> retvalue = new List<Vector2>();

            float dx = groups[0].X, dy = groups[0].Y;
            foreach (Vector2 v in groups)
            {
                if (v.X < dx)
                    dx = v.X;
                if (v.Y < dy)
                    dy = v.Y;
            }
            foreach (Vector2 v in groups)
            {
                retvalue.Add(new Vector2(v.X - dx, v.Y - dy));
            }
            return retvalue;
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

        private float strokeLength(List<Vector2> data)
        {
            double total = 0;
            for (int i = 0; i < data.Count - 1; i++)
                total += distance(data[i], data[i + 1]);

            return (float)total;
        }

        private float FNGREMAINDER;

        private Vector2 getPoint(Vector2 v1, Vector2 v2, float dist, out float remainder)
        {
            Vector2 direction = (v2 - v1) / (float) magnitude(v2 - v1);
            Vector2 length = direction * dist;
            remainder = (float)distance(v1 + length, v2);
            return v1 + length;
        }
            
            // there are some possibilities for this to cause errors
            // note that this is not necessarily a completely ROBUST function
            // #testingphase
        private Vector2 findNextGroup(float dist, List<Vector2> data, ref int last)
        {
            if (last == 0)
                FNGREMAINDER = 0;

            float tdist = FNGREMAINDER;
            FNGREMAINDER = 0;

            while (last < data.Count - 1)
            {
                float inD = (float)distance(data[last], data[last + 1]);
                last++;
                if (tdist + inD > dist)
                    break;
                tdist += inD;
            }

            return getPoint(data[last - 1], data[last], dist - tdist, out FNGREMAINDER);
        }

        private List<float> getDistances(List<Vector2> data)
        {
            float datalength = strokeLength(data);
            float pointlength = strokeLength(points);

            List<float> distances = new List<float>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                float dist = (float)distance(points[i], points[i + 1]);
                distances.Add(datalength * dist / pointlength);
            }

            return distances;
        }

        private List<Vector2> createGroups(List<Vector2> data)
        {
            List<float> gDistance = getDistances(data);



            List<Vector2> groups = new List<Vector2>();

                // the first point in the list must be a group since it is the start
                // of the line
            groups.Add(data[0]);

                // here we find the groups inbetween
            int last = 0;
            groups.Add(findNextGroup(gDistance[0], data, ref last));
            for (int i = 0; i < numGroups - 3; i++)
                groups.Add(findNextGroup(gDistance[i + 1], data, ref last));
                // the last point in the list must be a group since it is the end of the
                // line
            groups.Add(data[data.Count - 1]);
            
            return groups;
        }



            // the goal of this function is to take our expected points and rescale it
            // so that we can have a more accurate analysis of the data
        private List<Vector2> reproportion(List<Vector2> groups)
        {
            List<Vector2> retvalue = new List<Vector2>();
            float dx = groups[0].X, dy = groups[0].Y;
            foreach (Vector2 v in groups)
            {
                if (v.X > dx)
                    dx = v.X;
                if (v.Y > dy)
                    dy = v.Y;
            }
            for (int i = 0; i < points.Count; i++)
            {
                retvalue.Add(new Vector2(points[i].X * dx, points[i].Y * dy));
            }
            return retvalue;
        }

        private List<Vector2> calcSlopeVector(List<Vector2> data)
        {
            List<Vector2> slopes = new List<Vector2>();

            for (int i = 0; i < data.Count - 1; i++)
                slopes.Add(new Vector2(data[i + 1].X - data[i].X, data[i + 1].Y - data[i].Y));

            return slopes;
        }

        private List<float> anglePercDiff(List<Vector2> l1, List<Vector2> l2)
        {
            List<float> percDiff = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                float angle = (float)Math.Acos(dotprod(l1[i], l2[i]) / (magnitude(l1[i]) * magnitude(l2[i])));
                if(float.IsNaN(angle) || float.IsInfinity(angle))
                    continue;
                percDiff.Add(angle * 180 / (float) Math.PI);
            }

            return percDiff;
        }

        private List<Vector2> calcRelationalVectors(List<Vector2> data, Vector2 p)
        {
            List<Vector2> relational = new List<Vector2>();
            foreach (Vector2 v in data)
                relational.Add(new Vector2(p.X - v.X, p.Y - v.Y));

            return relational;
        }

        private List<float> comparePositions(List<Vector2> l1, List<Vector2> l2)
        {
            List<float> dpos = new List<float>();

            float radius = 0;
            foreach (Vector2 v in l1)
            {
                if (v.X > radius)
                    radius = v.X;
                if (v.Y > radius)
                    radius = v.Y;
            }
            radius /= 10; // we want 10%

            for (int i = 0; i < l1.Count; i++)
            {
                float diff = (float)distance(l1[i], l2[i]);
                dpos.Add(diff / radius);
            }

            return dpos;
        }

        private float analyze(List<Vector2> groups)
        {
            groups = align(groups);
            List<Vector2> expect = reproportion(groups);

            List<Vector2> eslopes = calcSlopeVector(expect);
            List<Vector2> gslopes = calcSlopeVector(groups);

            List<float> percentDiff = anglePercDiff(expect, groups);

            List<Vector2> erelational = calcRelationalVectors(expect, Vector2.Zero);
            List<Vector2> grelational = calcRelationalVectors(groups, Vector2.Zero);

            List<float> relationDiff = anglePercDiff(erelational, grelational);

            List<float> posDiff = comparePositions(expect, groups);

            float sum = percentDiff.Sum();
            float rsum = relationDiff.Sum();

            return sum;
        }
    }
}
