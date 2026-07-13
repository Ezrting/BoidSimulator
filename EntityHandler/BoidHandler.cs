using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.EntityHandler
{
    static class BoidHandler
    {
        public static List<Entity> ListOfBoids = new();
        public static Vector2 Rule1(Entity TheBoid) //Cohesion
        {
            Vector2 AveragePosition = Vector2.Zero;
            float ExtraWeight = 0;
            foreach (Entity B in ListOfBoids)
            {
                if (B != TheBoid)
                {
                     float SideLengthDifference = (B.SideLength * B.SideLength) / (TheBoid.SideLength * TheBoid.SideLength); //Boids tend to follow larger boids. Its exponential since its in 2 dimensions
                     float AddedWeight = SideLengthDifference / 10;
                     ExtraWeight += AddedWeight;

                    AveragePosition += B.Origin * (1 +AddedWeight);
                }
            }
            AveragePosition /= ListOfBoids.Count - 1 + ExtraWeight;
            return (AveragePosition - TheBoid.Origin)/ 20;
        }
        public static Vector2 Rule2(Entity TheBoid) //Seperation
        {
            Vector2 C = Vector2.Zero;
            foreach (Entity B in ListOfBoids)
            {
                if (B != TheBoid)
                {
                    if (Vector2.Distance(B.Origin, TheBoid.Origin) < TheBoid.SideLength*3)
                    {
                        C -= (B.Origin - TheBoid.Origin);
                    }
                }
            }
            return C;
        }
        public static Vector2 Rule3(Entity TheBoid)
        {
            Vector2 PercievedVelocity = Vector2.Zero;
            foreach (Entity B in ListOfBoids)
            {
                if (B != TheBoid)
                {
                    PercievedVelocity += B.Velocity;
                }
            }
            PercievedVelocity /= ListOfBoids.Count - 1;
            return (PercievedVelocity - TheBoid.Velocity) / 8;
        }
        public static Vector2 BoundaryRule(Entity TheBoid) //Boids stay within a set bounds
        {
            int Xmin = -1800;
            int Xmax = 3600;
            int Ymin = -1800;
            int Ymax = 3600;
            Vector2 V = Vector2.Zero;
            bool IsOutOfBounds = false;
            if (TheBoid.Origin.X < Xmin)
            {

                IsOutOfBounds = true;
                V.X = 100;
            }
            if (TheBoid.Origin.X > Xmax)
            {
                IsOutOfBounds = true;
                V.X = -100;
            }
            if (TheBoid.Origin.Y < Ymin)
            {
                IsOutOfBounds = true;
                V.Y = 100;
            }
            if (TheBoid.Origin.Y > Ymax)
            {
                IsOutOfBounds = true;
                V.Y = -100;
            }
            if (IsOutOfBounds) TheBoid.Properties.IsOutOfBounds = true;
            else TheBoid.Properties.IsOutOfBounds = false;

            return V;
        }
    }
}
