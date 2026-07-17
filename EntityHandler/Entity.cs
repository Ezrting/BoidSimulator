using Boid_Simulator;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using System.Diagnostics;
using Boid_Simulator.GUI;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using Boid_Simulator.Utility;

namespace Boid_Simulator.EntityHandler
{
    public class EntityProperties // Bonus properties that can be added on top of existing properties
    {
        public bool IsOutOfBounds = false;
        public double AngleToTarget = 0;
        public EntityProperties()
        {
        }
    }
    public class Entity
    {
        //Static variables
        public static double TotalRadiansRotation;
        public static int TotalRotationCount;
        public static List<Entity> UniversalEntityList = new List<Entity>();
        //Basic variables
        public Player ControllingPlayer = null;
        public string ID;
        public EntityProperties Properties = null;
        //Positional variables
        public Vector2 Origin = new Vector2(200, 400);
        public float SideLength = 10;

        public float Orientation = 0; //in radians
        float OrientationOffset = (float)Math.PI / 2; //Used solely to MOVE the boid when its design means its facing the wrong way.
        public Vector2 Velocity = Vector2.Zero;
        public float RotateSpeed = 0.5f;
        public float Speed;
        Vector2[] PositionList;
        public Shape PrimaryShape;
        List<VisualisedLine> ViewLinesToDestroy = new();
        public Entity(string id, Vector2 origin, float sideLength, Shape _shape = null)
        {

            SideLength = sideLength;
            Origin = origin;
            this.PrimaryShape = _shape ?? new();
            this.PrimaryShape.Owner = this;
            this.ID = id;
            Properties = new();
            UniversalEntityList.Add(this);
            BoidHandler.ListOfBoids.Add(this);
        }
        float PreviousAngleGap = 0;
        float PreviousLineCount = 0;
        float PreviousLineLength = 0;
        public static Entity FindByName(string Name)
        {
            return UniversalEntityList.Find(e => e.ID == Name); // e is placed before => so it is a parameter.
        }
        public void AdjustBoidGeometry()
        {

            PositionList = new Vector2[] { new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength) };
            float cos = (float)Math.Cos(Orientation);
            float sin = (float)Math.Sin(Orientation);
            Origin += new Vector2(0.0f, 0);
            //Move in direction Boid is facing
            for (int i = 0; i < PositionList.Length; i++)
            {
                Vector2 Pivot = PositionList[i] - new Vector2(0, 0); // Negate the point minus the point you are turning around. Although note in this case PositionList[i] is relative to the centre right now, which
                                                                     //means the pivot is (0,0). This does make things easier but is important to keep in mind.
                PositionList[i] = new Vector2(cos * Pivot.X - sin * Pivot.Y, sin * Pivot.X + cos * Pivot.Y);
                //I used AI, but turns out this is the rotation matrix
            }
            for (int i = 0; i < PositionList.Length; i++)
            {
                PositionList[i] += Origin;
            }
            if (PrimaryShape != null)
            {
                PrimaryShape.CreateLines(PositionList);
            }
        }
        public void CreateViewLines(float AngleGap, float LineCount, float LineLength)
        {
            bool Visualise = false;
            bool ReusingLines = true;
            if (AngleGap != PreviousAngleGap || LineCount != PreviousLineCount || LineLength != PreviousLineLength)
            {
                ReusingLines = false;
                PreviousAngleGap = AngleGap;
                PreviousLineCount = LineCount;
                PreviousLineLength = LineLength;
                foreach (VisualisedLine line in ViewLinesToDestroy)
                {
                    VisualisedLine.ListOfLines.Remove(line);

                }
            }
            Vector2 EyePosition = Origin; ///Placeholder until we build an entire custom boid builder.
            float StartingAngle = Orientation + OrientationOffset/1.6f - ((LineCount - 1) / 2) * AngleGap ;
            for (int i = 0; i < LineCount; i++)
            {
                double cos = Math.Cos(StartingAngle);
                double sin = Math.Sin(StartingAngle);
                Vector2 Pivot = EyePosition;
                Vector2 Direction = new Vector2((float)cos, (float)sin);

                Vector2 NewPos = Pivot + Direction * LineLength;
                if (Visualise)
                {
                    if (ReusingLines == false)
                    {
                        VisualisedLine VL = new(Pivot, NewPos);
                        ViewLinesToDestroy.Add(VL); //must destroy next time CreateViewLines is called
                        VL.Width = 1;
                    }
                    else
                    {
                        VisualisedLine VL = ViewLinesToDestroy[i];
                        VL.StartPos = Pivot;
                        VL.EndPos = NewPos;
                    }
                }
                foreach (Entity entity in UniversalEntityList)
                {
                    if (entity.ID != this.ID && entity.PrimaryShape != null && entity.PrimaryShape.Lines != null)
                    {
                        foreach (Line line in entity.PrimaryShape.Lines)
                        {
                            foreach (VisualisedLine VL in ViewLinesToDestroy)
                            {
                                if (Line.FindIntersection(VL, line, out Vector2? Intersection))
                                {
                                    VL.EndPos = Intersection.Value;
                                }
                            }
                        }
                    }
                }
                //Line.FindIntersection(VL, Line line2, out Vector2? Intersection)
                StartingAngle += AngleGap;
            }
        }
        public void Move(double Amount, float? CustomOrientation = null, float ? Cos = null, float? Sin = null)
        {
            float orientation = CustomOrientation ?? Orientation + OrientationOffset; // if no custom value is provided, CustomOrientation resets to the shape's current orientation
            //Move in the set direction;
            double cos = Cos ?? Math.Cos(orientation); // Get the provided cosine if it has been set
            double sin = Sin ?? Math.Sin(orientation);
            Velocity = new Vector2((float)(Amount * cos), (float)(Amount * sin));
            Origin += Velocity;
        }
        public void MoveToPoint(Vector2 NewPosition, float? CustomOrientation = null)
        {
            float orientation = CustomOrientation ?? Orientation + OrientationOffset; // if no custom value is provided, CustomOrientation resets to the shape's current orientation
            Orientation = orientation;
            Origin = NewPosition;
        }
        public void Update(GameTime gameTime)
        {
            double DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 AimedVelocity = Vector2.Zero;
            float desiredSpeed;
            if (ControllingPlayer != null)
            {
                desiredSpeed = Speed;

            }
            else
            {
                Vector2 v1 = BoidHandler.Rule1(this);
                Vector2 v2 = BoidHandler.Rule2(this);
                Vector2 v3 = BoidHandler.Rule3(this);
                Vector2 BoundaryRule = BoidHandler.BoundaryRule(this);
                AimedVelocity = v1 + v2 + v3;// + BoundaryRule;
                desiredSpeed = Math.Min(AimedVelocity.Length(), Speed);
                //won't go beyond max speed
                //Only compute when there is meaningful desired velocity
                if (MathUtil.SquaredDistance(AimedVelocity, Vector2.Zero) > 1e-6f)
                {
                    //Account for orientation offset. This is what the orientation needs to reach, it needs not worry of the OrientationOffset as it is added during
                    
                    Properties.AngleToTarget = MathUtil.NormaliseAngle( Math.Atan2(AimedVelocity.Y, AimedVelocity.X) - OrientationOffset);
                    
                    double AngleDifference = Properties.AngleToTarget - Orientation;
                    AngleDifference = MathUtil.NormaliseAngle(AngleDifference);
                    //Max allowed rotation this frame, in radians, always positive.
                    double MaxRotation = DeltaTime * RotateSpeed * (float)Math.PI / 180; // Converted to radians
                    double Clamped = Math.Max(-MaxRotation, Math.Min(MaxRotation, AngleDifference));
                    TotalRadiansRotation += Clamped;
                    TotalRotationCount++;
                    Orientation += (float)Clamped; // in radians
                }
            }




            AdjustBoidGeometry();
            Move(desiredSpeed * DeltaTime);
           // CreateViewLines(8 * (float)Math.PI / 180f, 10, 10 * SideLength);
        }
    }
    static class EntityFunctions //Entity functions that help handle entities
    {
        public static void Update(GameTime gameTime)
        {
           // Debug.WriteLine(Entity.TotalRadiansRotation / Entity.TotalRotationCount);
            foreach (Entity entity in Entity.UniversalEntityList)
            {
                entity.Update(gameTime);
            }
        }
    }

}
