using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Boid_Simulator.EntityHandler;
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
        //Following function's purpose, despite its intimidating name, is to hold items that need to be added in lists in place, but only add them when the time is right
        //(aka at the end of the frame.) this is to prevent race conditions and that stupid enumeration exception
        public static class DeferredCollectionManager<T>
        //Generic clases are marked with <generic variable name>, usually marked with <T>.
        //<T> is a value that works with objects
        //not to be confused with object, compile-time and the type of its value can be changed after declaration:
        //e.g. object foo = 5;
        //foo ' "hi";
        //meanwhile var is just an implicit way to declare a type e.g. var foo = 5;
        {
            static readonly object _lock = new();
            //
            static readonly Dictionary<IList<T>, List<T>> ToAdd = new(); //Lists paired with the items that need to be added to them
            static readonly Dictionary<IList<T>, List<T>> ToRemove = new();//Lists paired with the items that need to be removed from them

            public static void BookAdd(IList<T> target, T item)
            {
                if (target == null) return; //if there is nothing to add to
                lock(_lock)
                {
                    if (!ToAdd.TryGetValue(target, out var DeferredList)) //try find the target list
                    {
                        //If not, redefine the currently null list that was just initialised.
                        DeferredList = new List<T>();
                        ToAdd[target] = DeferredList;
                    }
                    DeferredList.Add(item);
                }
            }

            public static void BookRemove(IList<T> target, T item)
            {
                if (target == null) return; //if there is nothing to remove from
                lock (_lock)
                {
                    if (!ToRemove.TryGetValue(target, out var DeferredList)) //try find the target list
                    {
                        //If not, redefine the currently null list that was just initialised.
                        DeferredList = new List<T>();
                        ToRemove[target] = DeferredList;
                    }
                    DeferredList.Add(item);
                }
            }
            public static void ApplyDeferred()
            {
                lock (_lock)
                {
                    foreach (var ListToAddTo in ToAdd)
                    {
                        //ListToAddTo is the type KeyValuePair<IList<T>, List<T>>.
                        //A KeyValuePair is simply a pair that represents a key and a value of a single dictionary item.
                        var target = ListToAddTo.Key;
                        foreach (var ItemToAdd in ListToAddTo.Value)
                        {
                            target.Add(ItemToAdd);
                        }
                      
                    }
                    foreach (var ListToAddTo in ToRemove)
                    {
                        //ListToAddTo is the type KeyValuePair<IList<T>, List<T>>.
                        //A KeyValuePair is simply a pair that represents a key and a value of a single dictionary item.
                        var target = ListToAddTo.Key;
                        foreach (var ItemToAdd in ListToAddTo.Value)
                        {
                            target.Add(ItemToAdd);
                        }
                    }
                    ToAdd.Clear();
                    ToRemove.Clear();
                }
            }
        }
        public static (List<string> Contents, float[] LineLengths) SplitTextIntoRows(SpriteFont font, string text, float FontSize, float BoxWidth)
        {
            //replace unsupported
            static string ReplaceUnsupportedChars(SpriteFont font, string input, char preferredReplacement = '?')
            {
                
                var chars = font.Characters;
                if (chars.Count <= 0) { return string.Empty; }
                char Replacement = preferredReplacement;
                if (!chars.Contains(Replacement))
                {
                    Replacement = chars[0];
                }
                var lookup = new HashSet<char>(chars); //A Hashset is a collection of unique items, so duplicate letters are removed
                var sb = new System.Text.StringBuilder(input.Length); //empty array-like object
                foreach (var c in input)
                {
                    sb.Append(lookup.Contains(c) ? c : Replacement);
                }
                return sb.ToString();

            }
            //Each new item represents a row of text
            List<string> SplitRows = new();
            float CurrentDrawPositionX = 0;
            string CurrentWordBeingFormed = string.Empty;
            string CurrentLineBeingFormed = string.Empty;
            string SafeText = ReplaceUnsupportedChars(font, text, preferredReplacement: '*');
            string[] SplitText = SafeText.Split(' ');
            foreach (string s in SplitText)
            {
                Vector2 WordTextDimensions = font.MeasureString(s.ToString()) * FontSize;
                //If the word overlaps
                if (s == string.Empty)
                {
                    WordTextDimensions = font.MeasureString(" ".ToString()) * FontSize;
                    //This above is done because s will have a space character added to it, which has positional value.
                    //string.Empty characters are created here because multiple spaces could be in a row.
                }
                if (CurrentDrawPositionX + WordTextDimensions.X > BoxWidth)
                {
                    if (WordTextDimensions.X > BoxWidth) //If the word on its own is more than the box width
                    {
                        foreach (char Character in s)
                        {
                            Vector2 CharTextDimensions = font.MeasureString(Character.ToString()) * FontSize;
                            

                            if (CharTextDimensions.X > BoxWidth && CurrentDrawPositionX == 0) //if the SINGULAR character is longer than the box width, just give up and let it in
                            {
                                SplitRows.Add(CurrentLineBeingFormed);
                                CurrentLineBeingFormed = string.Empty;
                                CurrentDrawPositionX = 0;
                            }
                            else if (CurrentDrawPositionX + CharTextDimensions.X > BoxWidth) //Reset if a character of the really long word overlaps.
                            {
                                SplitRows.Add(CurrentLineBeingFormed);
                                CurrentLineBeingFormed = string.Empty;
                                CurrentDrawPositionX = 0;
                            }
                            CurrentLineBeingFormed += Character;
                            CurrentDrawPositionX += CharTextDimensions.X;
                        }
                    }
                    else
                    {
                        //Add a new line and start over.
                        SplitRows.Add(CurrentLineBeingFormed);
                        CurrentLineBeingFormed = string.Empty;
                        CurrentDrawPositionX = 0;
                    }

                }
                if (WordTextDimensions.X <= BoxWidth) //In the case where WordTextDimensions.X > BoxWidth, that exception has already been handled above
                {
                    //Add the word to the new line
                    CurrentLineBeingFormed += s + " ";
                    CurrentDrawPositionX += WordTextDimensions.X;
                }
                    
            }
           
            SplitRows.Add(CurrentLineBeingFormed);
            float[] LineLengths = new float[SplitRows.Count]; //This LineLengths does not refer to the named return type, nor does the program know it exists.
            //Naming return types is only useful to access the result by keys.
            for (int i = 0; i < SplitRows.Count; i++)
            {
                Vector2 LineDimensions = font.MeasureString(SplitRows[i]) * FontSize;
                LineLengths[i] = LineDimensions.X;
            }
            return (SplitRows, LineLengths);
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
