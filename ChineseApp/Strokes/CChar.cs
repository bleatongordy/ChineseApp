using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseApp.Strokes
{
    public class CChar
    {
        private List<StrokeType> StrokeTypeList;
        public List<Vector2> MidPointList;

        public CChar(List<StrokeType> stroketypelist, List<Vector2> midpointlist)
        {
            StrokeTypeList = stroketypelist;
            MidPointList = midpointlist;
        }

        public CChar(List<Stroke> strokelist)
        {
            StrokeTypeList = new List<StrokeType>();
            MidPointList = new List<Vector2>();

            foreach (Stroke s in strokelist)
            {
                StrokeTypeList.Add(s.StrokeType);
                MidPointList.Add(s.Midpoint);
            }
        }

        public CChar(List<List<Vector2>> strokedata)
        {
            List<Stroke> strokelist = new List<Stroke>();
            foreach (List<Vector2> v in strokedata)
            {
                strokelist.Add(new Stroke(v));
            }

            StrokeTypeList = new List<StrokeType>();
            MidPointList = new List<Vector2>();

            foreach (Stroke s in strokelist)
            {
                StrokeTypeList.Add(s.StrokeType);
                MidPointList.Add(s.Midpoint);
            }
        }

        public CChar()
        {
            StrokeTypeList = new List<StrokeType>();
            MidPointList = new List<Vector2>();
        }

        public void addStroke(Stroke s)
        {
            StrokeTypeList.Add(s.StrokeType);
            MidPointList.Add(s.Midpoint);
        }

        public override string ToString()
        {
            string retvalue = "";
            for (int i = 0; i < StrokeTypeList.Count - 1; i++)
            {
                retvalue += ((int)StrokeTypeList[i]).ToString() + "~";
            }
            retvalue += ((int)StrokeTypeList[StrokeTypeList.Count - 1]).ToString() + "|";
            for (int i = 0; i < MidPointList.Count - 1; i++)
            {
                retvalue += MidPointList[i].X.ToString() + "," + MidPointList[i].Y.ToString() + ":";
            }
            retvalue += MidPointList[MidPointList.Count - 1].X.ToString() + "," + MidPointList[MidPointList.Count - 1].Y.ToString();
            return retvalue;
        }

        public static CChar Parse(string data)
        {
            List<StrokeType> stroketypelist = new List<StrokeType>();
            List<Vector2> midpointlist = new List<Vector2>();

            string[] dp = data.Split('|');
            string[] stp = dp[0].Split('~');
            foreach (string s in stp)
            {
                stroketypelist.Add((StrokeType)int.Parse(s));
            }
            string[] mdp = dp[1].Split(':');
            foreach (string s in mdp)
            {
                string[] pdp = s.Split(',');
                midpointlist.Add(new Vector2(float.Parse(pdp[0]), float.Parse(pdp[1])));
            }

            return new CChar(stroketypelist, midpointlist);
        }
    }
}
