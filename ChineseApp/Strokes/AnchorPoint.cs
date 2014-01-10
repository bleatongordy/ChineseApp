using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseApp.Strokes
{
    /* The AnchorPoint class is be a special type of point. The purpose of
     * this class is to help define strokes. The way we do this is to
     * precreate a set of AnchorPoints that represent a point on a graph and a
     * slope at that representation. This way when we get incoming stroke data
     * we can model it based off of this approach. We subdivide the stroke data
     * into the different anchor points and do a match. If the slopes of the
     * stroke data does not match the strokes of the AnchorPoints than we can
     * assume (based off a confidence level) that the stroke is not this one.
     * 
     * Here is an example.
     * 
     * This is the character 了, we place anchor points at different slope intervals
     * 
     *         (S)    (M)    (E)(S)
     *          --------------
     *                     --           Actually, this is a bad representation
     *                   --(M)          because we won't have anchor points for
     *                 --               individual characters, just strokes.
     *                -(E)(S)
     *                -                 (S) Represents a start of an AnchorPoint
     *                -                 (M) Represents the middle of one
     *                -(M)              (E) Represents the end of one
     *        (E)-    -
     *          (M)-  -                 For curved representations we will have
     *         (E)(S)---(E)(S)          a (LC), left curve, or a (RC), right
     *               (M)                curve, this will alows us to have a 
     *                                  variance from the mean.
    */

    public enum AnchorPointType
    {
        AP_INVALID = -1,
        AP_START,
        AP_MID,
        AP_END,
        AP_LEFTCURVE,
        AP_RIGHTCURVE
    };

    public class AnchorPoint
    {
        float X, Y, Slope;
        AnchorPointType Type;

        public AnchorPoint(float x, float y, float slope, AnchorPointType t)
        {
            X = x;
            Y = y;
            Slope = slope;
            Type = t;
        }
    }

    public class Anchor
    {
        private AnchorPoint[] anchorPoints;
        public AnchorPoint[] AnchorPoints
        { get { return anchorPoints; } }
        
        private Rectangle dimensions;
        public Rectangle Dimensions
        { get { return dimensions; } }

        public Anchor(AnchorPoint[] anchorpoints, Rectangle dim)
        {
            anchorPoints = anchorpoints;
            dimensions = dim;
        }
    }
}
