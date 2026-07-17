using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Boid_Simulator.Utility;

namespace Boid_Simulator
{
    public class Line
    {
        Shape ShapeOwner = null;
        public Vector2 StartPos;
        public Vector2 EndPos;
        public static List<Line> ListOfLines = new();
        public float Gradient
        {
            get {
                if (StartPos.X == EndPos.X) return float.PositiveInfinity;
                return StartPos.Y == EndPos.Y ? 0 : (EndPos.Y - StartPos.Y) / (EndPos.X - StartPos.X); 
            }
        }
        public float YIntercept
        {
            get { return StartPos.Y - Gradient * StartPos.X; }
        }

        public float Length
        {
            get { return Vector2.Distance(StartPos, EndPos); }
        }
        public static bool FindIntersection(Line line1, Line line2, out Vector2? Intersection)
        {
            Intersection = null;

            Vector2 p = line1.StartPos;
            Vector2 r = line1.EndPos - line1.StartPos;//Startpos pointing to end;
            Vector2 q = line2.StartPos;
            Vector2 s = line2.EndPos - line2.StartPos;//Startpos pointing to end;
            //2D CrossProduct
            Vector2 qMinusP = q - p;
            float CrossR_S = MathUtil.TwoDCrossProduct(r, s);
            float CrossQmP_S = MathUtil.TwoDCrossProduct(qMinusP, s);
            float CrossQmP_R = MathUtil.TwoDCrossProduct(qMinusP, r);

            if (Math.Abs(CrossR_S) < float.Epsilon)
            {
                if (Math.Abs(CrossQmP_R) < float.Epsilon)
                {
                    //Collinear, could overlap
                    return false;
                }
                //Parallel and do not intersect;
                return false;
            }
            //parametric factors
            float t = CrossQmP_S / CrossR_S; //Along R
            float u = CrossQmP_R / CrossR_S; //Along S
            //Intersection occurs when t and u are in [0,1]
            if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
            {
                Vector2 point = p + t * r;
                //q + u * s = p + t * r
                Intersection = point;
                return true;
            }
            return false;
        }
        public Line(Vector2 startPos, Vector2 endPos, Shape shapeOwner = null)
        {
            this.StartPos = startPos;
            this.EndPos = endPos;
            this.ShapeOwner = shapeOwner;
            ListOfLines.Add(this);
        }

    }
    public class VisualisedLine : Line
    {
        public new static List<VisualisedLine> ListOfLines = new(); //new keyword on initialising hides previous static field
        public float Width = 1f;
        public Color DrawColour = Color.Yellow;
        public float Transparency = 0f; //Fully opaque
        public VisualisedLine(Vector2 startPos, Vector2 endPos) : base(startPos, endPos)
        {
            ListOfLines.Add(this);
        }
    }
}
