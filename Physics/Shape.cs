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
        public Shape(Vector2[] Positions)
        {
            Lines = new Line[Positions.Length];
            if (Positions.Length < 3) throw new ArgumentException("Shape needs at least 3 points to form");
            for (int i = 0; i < Positions.Length; i++)
            {
                Line line = new(Positions[i],(i == Positions.Length - 1) ? Positions[0] : Positions[i + 1]);
                Lines[i] = line;
            }
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
