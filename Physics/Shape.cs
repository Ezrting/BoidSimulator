using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Boid_Simulator
{
    public class Shape
    {
        public static List<Shape> ListOfShapes = new();

        public Line[] Lines;
        public float? Radius;
        public Shape(Line[] lines)
        {
            this.Lines = lines;
            Init();
        }
        public Shape(float radius)
        {
            this.Radius = radius;
            Init();
        }
        public void Init()
        {
            ListOfShapes.Add(this);
        }
        public static bool CheckIfPointInShape(Vector2 Point, Shape ShapeToCheck)
        {
            return false;
        }
    }
}
