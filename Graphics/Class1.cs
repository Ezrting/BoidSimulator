using Boid_Simulator.GUI;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator
{
    internal class Graphics
    {
        public static GraphicsDevice _graphicsDevice;
        public static Texture2D Pixel;
        public static Fonts GraphicalFonts;
        public static void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            GraphicalFonts = new Fonts();
        }
        public static Shape[] AcquireShapes()
        {
            return [.. Shape.ListOfShapes];
        }
        public static void MapPixelsToColours(Shape[] ShapesToSearchFrom, out Color[,] ColourMap, SpriteBatch SpriteBatch)
        {
            int GraphicsHeight = _graphicsDevice.Viewport.Height;
            int GraphicsWidth = _graphicsDevice.Viewport.Width;
            for (int i = 0; i < ShapesToSearchFrom.Length; i++)
            {
                Shape shape = ShapesToSearchFrom[i];
                float MaxY = float.MinValue;
                float MinY = float.MaxValue;
                float MaxX = float.MinValue;
                float MinX = float.MaxValue;
                for (int j = 0; j < shape.Lines.Length; j++)
                {
                    Line line = shape.Lines[j];
                    MaxY = (int)Math.Max(MaxY, Math.Max(line.StartPos.Y, line.EndPos.Y));
                    MinY = (int)Math.Min(MinY, Math.Min(line.StartPos.Y, line.EndPos.Y));

                    MaxX = (int)Math.Max(MaxX, Math.Max(line.StartPos.X, line.EndPos.X));
                    MinX = (int)Math.Min(MinX, Math.Min(line.StartPos.X, line.EndPos.X));
                }
                List<float>[] YPixelPositionsToSearchFrom = new List<float>[(int)(MaxX - MinX) + 1]; //A fixed array of a list of floats
                for (int PixelX = (int)MinX; PixelX <= (int)MaxX; PixelX++)
                {
                    int Index = PixelX - (int)MinX;
                    for (int j = 0; j < shape.Lines.Length; j++)
                    {
                        Line line = shape.Lines[j];
                        int lineMinX = (int)Math.Floor(Math.Min(line.StartPos.X, line.EndPos.X));
                        int lineMaxX = (int)Math.Ceiling(Math.Max(line.StartPos.X, line.EndPos.X));
                        Debug.WriteLine(PixelX + "," + lineMinX + "," + lineMaxX);
                        if (PixelX < lineMinX && PixelX > lineMaxX) // Boundary check!
                        {
                            continue;
                        }
                        float YPos = line.Gradient * PixelX + line.YIntercept;

                        if (YPixelPositionsToSearchFrom[Index] == null)
                        {
                            YPixelPositionsToSearchFrom[Index] = new List<float>();
                        }
                        YPixelPositionsToSearchFrom[Index].Add(YPos); // Add the intersection to the list of intersections for this PixelX value.

                    }
                    if (YPixelPositionsToSearchFrom[Index] == null)
                    {
                        continue;
                    }
                    YPixelPositionsToSearchFrom[Index].Sort((a, b) => a.CompareTo(b));
                    foreach(float YPos in YPixelPositionsToSearchFrom[Index])
                    {
                        //Debug.Write(YPos + ", ");
                    }
                   // Debug.WriteLine("");
                    for (int Intersection = 0; Intersection < YPixelPositionsToSearchFrom[Index].Count; Intersection++)
                    {
                        if (Intersection == YPixelPositionsToSearchFrom[Index].Count - 1)
                        {
                            break;
                        }
                       // Debug.WriteLine(Intersection);
                        if (Intersection %2 == 1) // Adds support for concave shapes //If intersection is odd, then inside the shape
                        {
                            float CurrentYPos = YPixelPositionsToSearchFrom[Index][Intersection]; // Current Ypos to draw from
                            float NextYPos = YPixelPositionsToSearchFrom[Index][Intersection + 1]; // Next Ypos, get the vertical distance to fill
                            Rectangle PixelRect = new Rectangle(
                                     (int)(PixelX - Game1.CameraCentre.X),
                                     (int)(CurrentYPos - Game1.CameraCentre.Y) + (int)Math.Abs(CurrentYPos - NextYPos), //Offset it by the distance from current to next, otherwise the shape will be inverted
                                     1,
                                     (int)(Math.Abs(CurrentYPos - NextYPos))); // The distance from current to next
                            //get size of screen to invert y axis
                            PixelRect.Y = GraphicsHeight - PixelRect.Y;
                            SpriteBatch.Draw(Pixel, PixelRect, null, Color.White);
                        }
                    }
                }
                
                
                /*
                for (int PixelX = (int)MinX; PixelX <= (int)MaxX; PixelX++)
                {
                    for (int PixelY = (int)MinY; PixelY <= (int)MaxY; PixelY++)
                    {
                        Rectangle PixelRect = new Rectangle(
                                     (int)(PixelX - Game1.CameraCentre.X),
                                     (int)(PixelY - Game1.CameraCentre.Y),
                                     5,
                                     5);
                        //get size of screen to invert y axis
                        PixelRect.Y = GraphicsHeight - PixelRect.Y;
                        SpriteBatch.Draw(Pixel, PixelRect, null, Color.White);
                    }
                }
                */

            }
            ColourMap = new Color[GraphicsWidth, GraphicsHeight];
            return;
        }

        public static void Draw(SpriteBatch SpriteBatch)
        {
            int GraphicsHeight = _graphicsDevice.Viewport.Height;
            int GraphicsWidth = _graphicsDevice.Viewport.Width;
            SpriteBatch.Begin();
            Shape[] ShapesToDraw = AcquireShapes();
            MapPixelsToColours(ShapesToDraw, out Color[,] ColourMap, SpriteBatch);
            for (int i = 0; i < ShapesToDraw.Length; i++)
            {
                Shape shape = ShapesToDraw[i];
                for (int j = 0; j < shape.Lines.Length; j++)
                {
                    Line line = shape.Lines[j];
                    Rectangle PixelRect = new Rectangle(
                        (int) (line.StartPos.X - Game1.CameraCentre.X), 
                        (int) (line.StartPos.Y - Game1.CameraCentre.Y), 
                        3, 
                        3);
                    //get size of screen to invert y axis
                    PixelRect.Y = GraphicsHeight - PixelRect.Y;
                    SpriteBatch.Draw(Pixel, PixelRect, null, Color.Red);
                    //SpriteBatch.Draw(Pixel, new Vector2(100,100) - Game1.CameraCentre, Color.White);
                }
            }
            for (int PixelX = 0; PixelX < GraphicsWidth; PixelX++)
            {
                for (int PixelY = 0; PixelY < GraphicsHeight; PixelY++)
                {

                }
            }
            //Draw TextBoxes
            foreach (TextBox textBox in TextBox.ListOfTextBoxes)
            {
                SpriteBatch.DrawString(GraphicalFonts.FontDictionary["Montserrat"], textBox.Text, textBox.ScreenPosition, Color.White);
            }
            SpriteBatch.End();
        }
    }
}
