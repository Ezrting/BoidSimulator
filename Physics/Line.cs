using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator
{
    public class Line
    {
        public Vector2 StartPos;
        public Vector2 EndPos;

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
        public Line(Vector2 startPos, Vector2 endPos)
        {
            this.StartPos = startPos;
            this.EndPos = endPos;
        }
    }
}
