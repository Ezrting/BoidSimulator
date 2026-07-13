using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.Utility
{
    internal class Util
    {
        public static float TwoDCrossProduct(Vector2 A, Vector2 B)
        {
            return A.X * B.Y - A.Y * B.X;
        }
        public static T GetItem<T>(T[] Array, int Index)
        {
            if (Index >= Array.Length)
            {
                return Array[Index % Array.Length]; // Loop back if the index is over than the length
            }
            else if (Index < 0)
            {
                return Array[Index % Array.Length + Array.Length]; //Also loop back, but add the length as the inital result will be a negative value.
            }
            else
            {
                return Array[Index];
            }
        }
        public static float SquaredDistance(Vector2 A, Vector2 B)
        {
            return (A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y);
        }
        public static double NormaliseAngle(double a)
        {
            //normalise into -Pi <= x <= Pi. This boundary makes sure boids don't rotate the long way round.
            while (a < -Math.PI)
            {
                a += 2 * Math.PI;
            }
            while (a > Math.PI)
            {
                a -= 2 * Math.PI;
            }
            return a;
        }
        public static float SimplifyDouble(double d, int DecimalPlaces)
        {
            int TensMultiplier = (int)Math.Pow(10, DecimalPlaces);
            return (float)Math.Floor(d * TensMultiplier) / TensMultiplier;
        }
    }
}
