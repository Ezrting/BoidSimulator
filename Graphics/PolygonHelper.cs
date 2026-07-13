using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Boid_Simulator.Utility;
using System.Globalization;

public enum WindingOrder
{
    Clockwise,
    CounterClockwise,
    Invalid
}
namespace Boid_Simulator.Graphics
{
    internal static class PolygonHelper
    {
        public static float FindPolygonArea(Vector2[] Vertices)
        {
            throw new NotImplementedException("This method is not implemented yet, but will be in the future");
        }
        public static bool Triangulate(Vector2[] Vertices, out int[] Triangles, out string ErrorMessage)
        {
            Triangles = null;
            ErrorMessage = string.Empty;
            if (Vertices == null)
            {
                ErrorMessage = "Vertices array is null.";
                return false;
            }
            if (Vertices.Length < 3)
            {
                ErrorMessage = "Vertices array is null.";
                return false;
            }
            if (Vertices.Length > 256)
            {
                ErrorMessage = "Too much vertices in the array, maximum length is 256.";
                return false;
            }
            if (ContainsColinearEdges(Vertices))
            {
                ErrorMessage = "Triangle contains colinear edges";
                return false;
            }
            if (!IsClockwise(Vertices))
            {
                Vertices.Reverse();
            }
            List<int> IndexList = new List<int>();
            for (int i = 0; i < Vertices.Length; i++)
            {
                IndexList.Add(i); //Add a number to represent the vertice in a list.
            }
            int TotalTriangleCount = Vertices.Length - 2;
            int TotalTriangleIndexCount = TotalTriangleCount * 3; //Length of the singular array used to store triangle indices, 3 for each triangle.

            Triangles = new int[TotalTriangleIndexCount];
            int TriangleIndexCount = 0;
            while (IndexList.Count > 3)
            {
                for (int i = 0; i < IndexList.Count; i++) //Loop through the list of numbers. each represents a vertice.
                {
                    bool IsEar = true;
                    int A = IndexList[i];
                    int B = Util.GetItem(IndexList.ToArray(), i - 1);
                    int C = Util.GetItem(IndexList.ToArray(), i + 1);
                    Vector2 VA = Vertices[A];
                    Vector2 VB = Vertices[B];
                    Vector2 VC = Vertices[C];
                    Vector2 AB = VB - VA;
                    Vector2 AC = VC - VA;
                    if (Util.TwoDCrossProduct(AC, AB) < 0f) // Skip triangles that are not concave(angle more than 180, and 180 is already banned anyway)
                    {
                        continue;
                    }
                    for (int j = 0; j < Vertices.Length; j++)
                    {
                        if (j == A || j == B || j == C) // Don't check the triangles points themselves
                        {
                            continue;
                        }
                        if (IsPointInTriangle(Vertices[j], VB, VA, VC)) // VB is first because you have to add clockwise to work.
                        {

                            IsEar = false;
                            break;
                        }
                    }
                    if (IsEar) 
                    {
                        Triangles[TriangleIndexCount++] = A;
                        Triangles[TriangleIndexCount++] = B;
                        Triangles[TriangleIndexCount++] = C;
                        IndexList.RemoveAt(i); // remove the Ith index. 
                        break; //Break out of the loop so it doesnt continue with a changed array(eventually causing indexoutofrange exception)
                    }
                }
            }
            //Add remaining 3 sides
            Triangles[TriangleIndexCount++] = IndexList[0];
            Triangles[TriangleIndexCount++] = IndexList[1];
            Triangles[TriangleIndexCount++] = IndexList[2];
            return true;
        }
        public static bool IsPointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
        {
            Vector2 AB = B - A;
            Vector2 BC = C - B;
            Vector2 CA = A - C;

            Vector2 AP = P - A;
            Vector2 BP = P - B;
            Vector2 CP = P - C;
            float CrossABAP = Util.TwoDCrossProduct(AB, AP);
            float CrossBCBP = Util.TwoDCrossProduct(BC, BP);
            float CrossCACP = Util.TwoDCrossProduct(CA, CP);
            if (CrossABAP > 0f || CrossBCBP > 0f || CrossCACP > 0f)
            {
                return false;
            }
            return true;
        }
        public static bool IsClockwise(Vector2[] Vertices)
        {
            float Sum = 0f;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2 v0 = Util.GetItem(Vertices, i);
                Vector2 v1 = Util.GetItem(Vertices, i + 1);
                Sum += Util.TwoDCrossProduct(v0, v1);
            }
            return Sum < 0f;
        }
        public static bool IsSimplePolygon(Vector2[] Vertices)
        {
            //Ensures a vertice isn't connected by more than 2 lines and that a vertice doesn't cross over itself when the lines are drawn.
            throw new NotImplementedException("This method is not implemented yet, but will be in the future");
        }
        public static bool ContainsColinearEdges(Vector2[] Vertices)
        {
            const float EPS = 0.001f;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2 A = Util.GetItem(Vertices, i);
                Vector2 B = Util.GetItem(Vertices, i - 1);
                Vector2 C = Util.GetItem(Vertices, i + 1);
                Vector2 AB = B - A;
                Vector2 AC = C - A;
                if (Math.Abs(Util.TwoDCrossProduct(AB, AC)) < EPS)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
