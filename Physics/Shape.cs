using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Boid_Simulator.GUI;
using Boid_Simulator.EntityHandler;

namespace Boid_Simulator
{
    public class Shape
    {
        public static List<Shape> ListOfShapes = new();

        public Line[] Lines;
        public Vector2[] Vertices;
        public float? Radius;
        public Color DrawColour = Color.FloralWhite;
        public float Transparency = 0f;
        public Entity? Owner;
        
        public Shape(Line[] lines)
        {
            this.Lines = lines;
            Init();
        }
        public Shape(Vector2[] Positions)
        {
            CreateLines(Positions);
            Init();
        }
        public Shape(float radius)
        {
            this.Radius = radius;
            Init();
        }
        public Shape()
        {
            Init();
        }
        public void Init()
        {
            ListOfShapes.Add(this);
        }
        public static bool PointInShape(Vector2 Point, Shape ShapeToCheck)
        {
            return false;
        }
        public void CreateLines(Vector2[] Positions)
        {
            if (Positions.Length < 3) throw new ArgumentException("Shape needs at least 3 points to form");
            Vertices = Positions;
            var newLines = new Line[Positions.Length]; //Create a new temporary array as if it this field accessed while the array is being created, it will create an error
            for (int i = 0; i < Positions.Length; i++)
            {
                newLines[i] = new(Positions[i], (i == Positions.Length - 1) ? Positions[0] : Positions[i + 1], this);
            }
            Lines = newLines;
            string PositionsString = "Created wonderful shape, Positions: ";
            for (int i = 0; i < Positions.Length; i++)
            {
                PositionsString += Positions[i];
            }
           // TextBox.StateByTextBox(PositionsString, new Vector2(10,30), "Blah");
        }
        public static bool CheckIfPointInShape(Vector2 Point, Shape ShapeToCheck)
        {
            return false;
        }
    }
}
