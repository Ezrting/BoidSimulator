using Boid_Simulator.GUI;
using Boid_Simulator.Utility;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.Graphics
{
    internal class Visualiser
    {
        public static GraphicsDevice _graphicsDevice;
        //public static GraphicsDeviceManager _graphics;
        public static Texture2D Pixel;
        private DynamicVertexBuffer _dynamicVertexBuffer;
        static BasicEffect _basicEffect;
        //Visualisation Parameters
        public static float Zoom = 1;
        //Convert world position to screen space with camera pos and zoom.
        public static void DrawUIElement(SpriteBatch _spriteBatch, UIElement element, Vector2 Position)
        {
            if (element is TextBox tb)
            {
                SpriteFont TextFont = Fonts.FontDictionary["Montserrat"];
                var Result = GeneralUtil.SplitTextIntoRows(TextFont, tb.Text, 1, tb.MaxBoxDimensions.X);
                List<string> TextLines = Result.Contents;
                float MaxHeight = tb.FontSize * TextLines.Count;
                //Adjust Transparency
                if (tb.BoxTransparency < 1)
                {
                    Vector2[] PositionList = { new Vector2(0, 0), new Vector2(tb.MaxBoxDimensions.X, 0), new Vector2(tb.MaxBoxDimensions.X, MaxHeight), new Vector2(0, MaxHeight) };
                    for(int i = 0; i < PositionList.Length; i++){ PositionList[i] += Position; }
                    tb.BoxShape = tb.BoxShape != null ? tb.BoxShape : new();
                    tb.BoxShape.CreateLines(PositionList);
                }
                if (tb.BoxShape != null)
                {
                    tb.BoxShape.DrawColour = tb.BoxColour;
                    tb.BoxShape.IsPhysical = false;
                    tb.BoxShape.Transparency = tb.BoxTransparency;
                }
                for (int i = 0; i < TextLines.Count; i++)
                {
                    string textline = TextLines[i];
                    if(tb.BoxShape != null) DrawShapes(new[] { tb.BoxShape }, false);
                    _spriteBatch.DrawString(TextFont, textline, Position + new Vector2(0, i * tb.FontSize), Color.White);
                }
            }
        } 
        public static void Init(GraphicsDeviceManager _graphics, GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            // Adjust the back buffer size
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 50;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 50;
            _graphics.ApplyChanges();
            //Set up and view projection matrices;
            _basicEffect = new BasicEffect(_graphicsDevice)
            {
                VertexColorEnabled = true, //Basically allows the drawing to produce colour
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateOrthographicOffCenter(
                    0,
                    _graphicsDevice.Viewport.Width,
                   _graphicsDevice.Viewport.Height,
                   0,
                   0,
                   1)
            };
           
        }
        public static Shape[] AcquireShapes(Predicate<Shape> WhitelistFunc) //Predicates are like Func<>, but they specifically return a true or false value only.
        {
            var UnfilteredShapeList = Shape.ListOfShapes;
            Shape[] WhitelistedArray = [.. UnfilteredShapeList.FindAll(WhitelistFunc)];
            foreach (Shape shape in UnfilteredShapeList)
            {
                if (shape.IsPhysical == true)
                {
                    //Debug.WriteLine("ERROR");
                }
            }
            Debug.WriteLine($" A whitelisted Shapes: {WhitelistedArray.Length}" + ", " + $" Whole shapes: {UnfilteredShapeList.Count}");
            return WhitelistedArray;
        }
        public static VisualisedLine[] AcquireVisualisedLines()
        {
            return [.. VisualisedLine.ListOfLines];
        }

        public static void DrawShapes(Shape[] ShapesToDraw, bool AccountSpace)
        {
            int GraphicsHeight = _graphicsDevice.Viewport.Height;
            int GraphicsWidth = _graphicsDevice.Viewport.Width;
            Vector2 CameraPos = GameIns.CameraPos;
            if (AccountSpace == true)
            {
                ////Debug
                //Color ShapeColour = Color.AliceBlue;
                //Vector3 dimensionA = new Vector3(
                //         0,
                //         0,
                //         0);
                //Vector3 dimensionB = new Vector3(
                //    100,
                //    100,
                //    0);
                //Vector3 dimensionC = new Vector3(
                //   100,
                //   -20,
                //    0);
                //var triangleVertices = new VertexPositionColor[]
                //      {
                //             new VertexPositionColor(dimensionA, ShapeColour),
                //             new VertexPositionColor(dimensionB, ShapeColour),
                //             new VertexPositionColor(dimensionC, ShapeColour),
                //      };
                //_graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleVertices, 0, 1);
            }
            for (int i = 0; i < ShapesToDraw.Length; i++)
            {
                Shape shape = ShapesToDraw[i];
                Color ShapeColour = shape.DrawColour;
                // if (shape.Owner != null && shape.Owner.Properties.IsOutOfBounds) ShapeColour = Color.DarkRed;
                byte alpha = (byte)(255 * (1f - shape.Transparency));
                ShapeColour = new Color(ShapeColour.R, ShapeColour.G, ShapeColour.B, alpha);
                //Fill in the shape
                bool TriangulationSuccess = PolygonHelper.Triangulate(shape.Vertices, out int[] Triangles, out string error);
                //Test



                if (TriangulationSuccess)
                {
                    for (int j = 0; j < Triangles.Length; j += 3)
                    {
                        Vector3 DimensionA = new Vector3(
                           shape.Vertices[Triangles[j]],
                           0);
                        Vector3 DimensionB = new Vector3(
                            shape.Vertices[Triangles[j + 1]],
                            0);
                        Vector3 DimensionC = new Vector3(
                           shape.Vertices[Triangles[j + 2]],
                            0);
                        if (AccountSpace)
                        {
                            Vector2 A = shape.Vertices[Triangles[j]] - CameraPos;
                            Vector2 B = shape.Vertices[Triangles[j + 1]] - CameraPos;
                            Vector2 C = shape.Vertices[Triangles[j + 2]] - CameraPos;

                            Vector2 CameraCentre = new Vector2(GraphicsWidth / 2, GraphicsHeight / 2);

                            Vector2 ZoomCentrePos = CameraCentre; // Pos to zoom on
                            Vector2 TranslatedA = A - ZoomCentrePos;
                            Vector2 TranslatedB = B - ZoomCentrePos;
                            Vector2 TranslatedC = C - ZoomCentrePos;
                            //This creates a vector from the zoom centre to the vertice
                            TranslatedA *= Zoom;
                            TranslatedB *= Zoom;
                            TranslatedC *= Zoom;
                            TranslatedA += ZoomCentrePos; // Add back the positions after the vectors have been scaled by the zoom
                            TranslatedB += ZoomCentrePos;
                            TranslatedC += ZoomCentrePos;

                            DimensionA = new Vector3(
                             TranslatedA.X,
                             TranslatedA.Y,
                             0);
                            DimensionB = new Vector3(
                                TranslatedB.X,
                                TranslatedB.Y,
                                0);
                            DimensionC = new Vector3(
                               TranslatedC.X,
                               TranslatedC.Y,
                                0);
                            // TextBox.StateByTextBox("Vector2 = 1", 
                        }

                        
                        var TriangleVertices = new VertexPositionColor[]
                        {
                             new VertexPositionColor(DimensionA, ShapeColour),
                             new VertexPositionColor(DimensionB, ShapeColour),
                             new VertexPositionColor(DimensionC, ShapeColour),
                        };
                        _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, TriangleVertices, 0, 1);
                    }

                }
                else
                {
                    Debug.WriteLine($"There was an error: {error}");
                }
                //Just draw the dots

            }
        }
        public static void Draw(SpriteBatch SpriteBatch)
        {
            int GraphicsHeight = _graphicsDevice.Viewport.Height;
            int GraphicsWidth = _graphicsDevice.Viewport.Width;

            Vector2 CameraPos = GameIns.CameraPos;
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
            Shape[] ShapesToDraw = AcquireShapes(X=> X.IsPhysical == true);
            VisualisedLine[] VisualisedLinesToDraw = AcquireVisualisedLines();
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _graphicsDevice.RasterizerState = RasterizerState.CullNone; // to prevent clockwise triangles from being invisible
            _graphicsDevice.BlendState = BlendState.AlphaBlend;
             DrawShapes(ShapesToDraw, true);
            //Fill in shape
            foreach (VisualisedLine vl in VisualisedLinesToDraw)
            {
                if (vl is VisualisedLine)
                {
                    Color LineColour = vl.DrawColour;
                    byte alpha = (byte)(255 * (1f - vl.Transparency));
                    LineColour = new Color(LineColour.R, LineColour.G, LineColour.B, alpha);
                    Vector2 p0 = vl.StartPos - GameIns.CameraPos;
                    Vector2 p1 = vl.EndPos - GameIns.CameraPos;
                    Vector2 Direction = vl.EndPos - vl.StartPos;
                    float len = Direction.Length();
                    if (len <= 0.0001f)
                    {
                        continue;
                    }

                    Vector2 N_Direction = Direction / len;
                    Vector2 perp = new Vector2(-N_Direction.Y, N_Direction.X); //Perpendicular line, specifically to the left.
                    float halfThickness = vl.Width / 2 > 1 ? vl.Width / 2 : 1f;
                 
                    Vector2 p0a = p0 + perp * halfThickness - CameraPos; // The camerapos needs to be accounted for when zooming
                    Vector2 p0b = p0 - perp * halfThickness - CameraPos;
                    Vector2 p1a = p1 + perp * halfThickness - CameraPos;
                    Vector2 p1b = p1 - perp * halfThickness - CameraPos;
                    Vector2 CameraCentre = new Vector2(GraphicsWidth / 2, GraphicsHeight / 2);

                    Vector2 ZoomCentrePos = CameraCentre; // Pos to zoom on
                    Vector2 Translated_p0a = p0a - ZoomCentrePos;
                    Vector2 Translated_p0b = p0b - ZoomCentrePos;
                    Vector2 Translated_p1a = p1a - ZoomCentrePos;
                    Vector2 Translated_p1b = p1b - ZoomCentrePos;
                    //This creates a vector from the zoom centre to the vertice
                    Translated_p0a *= Zoom;
                    Translated_p0b *= Zoom;
                    Translated_p1a *= Zoom;
                    Translated_p1b *= Zoom;
                    Translated_p0a += ZoomCentrePos;
                    Translated_p0b += ZoomCentrePos;
                    Translated_p1a += ZoomCentrePos;
                    Translated_p1b += ZoomCentrePos;
                    // Add back the positions after the vectors have been scaled by the zoom

                    var vertices = new VertexPositionColor[3];
                    vertices[0] = new VertexPositionColor(new Vector3(Translated_p0a, 0), LineColour);
                    vertices[1] = new VertexPositionColor(new Vector3(Translated_p0b, 0), LineColour);
                    vertices[2] = new VertexPositionColor(new Vector3(Translated_p1a, 0), LineColour);
                    var vertices2 = new VertexPositionColor[3];
                    vertices2[0] = new VertexPositionColor(new Vector3(Translated_p0b, 0), LineColour);
                    vertices2[1] = new VertexPositionColor(new Vector3(Translated_p1b, 0), LineColour);
                    vertices2[2] = new VertexPositionColor(new Vector3(Translated_p1a, 0), LineColour);

                    //Submit 2 triangles to the GPU.
                    _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
                    _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices2, 0, 1);
                    //SpriteBatch.Draw(Pixel, new Rectangle((int)p0.X, (int)p0.Y, 50, 50), LineColour);
                    //SpriteBatch.Draw(Pixel, new Rectangle((int)p1.X, (int)p1.Y, 50, 50), LineColour);
                }
            }
            SpriteBatch.Begin();
            //Draw TextBoxes
            foreach (UIElement element in UIElement.ListOfUIElements)
            {
                if (UIList.ListOfUILists.Any(x => x.Contents.Any(a => (a.Name == element.Name))) == false) // If could not find name
                {
                    DrawUIElement(SpriteBatch, element, element.ScreenPosition);
                }
                
            }
            foreach (UIList list in UIList.ListOfUILists)
            {
                Vector2 Position = list.StartingPosition;
                for (int i = 0; i < list.Contents.Count; i++)
                {
                    UIElement element = list.Contents[i];
                    Vector2 PositionOffset = new(0, i * 10);
                    DrawUIElement(SpriteBatch, element, Position + PositionOffset);
                   // SpriteBatch.DrawString(GraphicalFonts.FontDictionary["Montserrat"], textBox.Text, textBox.ScreenPosition + ScreenOffset, Color.White);
                }
            }
            SpriteBatch.End();
        }
    }
}
