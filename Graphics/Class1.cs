using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator
{
    public class Graphics
    {
        public static GraphicsDevice _graphicsDevice;
        public static Texture2D Pixel;
        public static void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
        public static Shape[] AcquireShapes()
        {
            return [.. Shape.ListOfShapes];
        }
        public static void MapPixelsToColours(Shape[] ShapesToSearchFrom, out Color[,] ColourMap)
        {
            int GraphicsHeight = _graphicsDevice.Viewport.Height;
            int GraphicsWidth = _graphicsDevice.Viewport.Width;
            List<Line> LinesToSearchFrom = new List<Line>();
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
                    MaxY = Math.Max(MaxY, Math.Max(line.StartPos.Y, line.EndPos.Y));
                    MinY = Math.Min(MaxY, Math.Min(line.StartPos.Y, line.EndPos.Y));

                    MaxX = Math.Max(MaxY, Math.Max(line.StartPos.X, line.EndPos.X));
                    MinX = Math.Min(MaxY, Math.Min(line.StartPos.X, line.EndPos.X));
                }
                for (int PixelX = (int)MinX; PixelX < (int)MaxX; PixelX++)
                {

                }
                
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
            for (int i = 0; i < ShapesToDraw.Length; i++)
            {
                Shape shape = ShapesToDraw[i];
                for (int j = 0; j < shape.Lines.Length; j++)
                {
                    Line line = shape.Lines[j];
                    Debug.WriteLine(line.EndPos);
                    Rectangle PixelRect = new Rectangle(
                        (int) (line.StartPos.X - Game1.CameraCentre.X), 
                        (int) (line.StartPos.Y - Game1.CameraCentre.Y), 
                        5, 
                        5);
                    //get size of screen to invert y axis
                    PixelRect.Y = GraphicsHeight - PixelRect.Y;
                    SpriteBatch.Draw(Pixel, PixelRect, null, Color.White);
                    //SpriteBatch.Draw(Pixel, new Vector2(100,100) - Game1.CameraCentre, Color.White);
                }
            }
            MapPixelsToColours(ShapesToDraw, out Color[,] ColourMap);
            for (int PixelX = 0; PixelX < GraphicsWidth; PixelX++)
            {
                for (int PixelY = 0; PixelY < GraphicsHeight; PixelY++)
                {

                }
            }
            SpriteBatch.End();
        }
    }
}
