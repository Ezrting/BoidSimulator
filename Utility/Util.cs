using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Boid_Simulator.Utility
{
    internal class MathUtil
    {
        public static float TwoDCrossProduct(Vector2 A, Vector2 B)
        {
            return A.X * B.Y - A.Y * B.X;
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
    internal class GeneralUtil
    {
        public static List<string> SplitTextIntoRows(SpriteFont font, string text, float FontSize, float BoxWidth)
        {
            //Each new item represents a row of text
            List<string> SplitRows = new();
            float CurrentDrawPositionX = 0;
            string CurrentWordBeingFormed = string.Empty;
            string CurrentLineBeingFormed = string.Empty;
            foreach (char Character in text)
            {
                Vector2 CharTextDimensions = font.MeasureString(Character.ToString()) * FontSize;
                CurrentDrawPositionX += CharTextDimensions.X;
                if (Character == ' ')
                {
                    Vector2 WordTextDimensions = font.MeasureString(CurrentWordBeingFormed.ToString()) * FontSize;
                    //If the word at its current position
                    if (CurrentDrawPositionX + WordTextDimensions.X > BoxWidth)
                    {
                        //Add the line and start over.
                        SplitRows.Add(CurrentLineBeingFormed);
                        CurrentLineBeingFormed = string.Empty;
                        CurrentDrawPositionX = 0;
                    }
                    else
                    {
                        //Reset the word
                        CurrentLineBeingFormed += CurrentWordBeingFormed + " ";
                        CurrentWordBeingFormed = string.Empty;
                        
                    }
                }
                else
                {
                    //Add character to current word being formed
                    CurrentWordBeingFormed += Character;
                    //If the character crosses the max border.
                    if (CurrentDrawPositionX + CharTextDimensions.X > BoxWidth)
                    {
                        //If the line is too big, splitting words is useless, so just start a new line.
                        if (CharTextDimensions.X > BoxWidth)
                        {
                            
                            SplitRows.Add(CurrentLineBeingFormed);
                            CurrentLineBeingFormed = string.Empty;
                            CurrentDrawPositionX = 0;
                        }
                    }
                }
            }
            SplitRows.Add(CurrentLineBeingFormed);
            return SplitRows;
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

        private static readonly Random random = new Random();
        public static string RandomID()
        {
            double RandomNumber = random.NextDouble();
            return random.NextDouble() + "-" + random.NextDouble();
        }
    }
}
