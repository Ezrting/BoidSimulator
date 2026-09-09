using Boid_Simulator.GUI;
using Boid_Simulator.Graphics;
using Boid_Simulator.EntityHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using Boid_Simulator.Utility;
using System;

namespace Boid_Simulator
{
    public class GameIns : Game
    {
        public bool Initialised = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Vector2 CameraPos;
        public static GameIns Instance;
        public static Player GameClient;
        public static List<Entity> BoidsBeingTracked = new();
        public GameIns()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
            GameClient = new(this);
           
            //change screen size to full screen
            //_graphics.IsFullScreen = true;
            // _graphics.HardwareModeSwitch = false;
            // _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Visualiser.Init(_graphics, GraphicsDevice);
            base.Initialize();
            Initialised = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Vector2 MousePos = Mouse.GetState().Position.ToVector2();
            int GraphicsHeight = GraphicsDevice.Viewport.Height;
            int GraphicsWidth = GraphicsDevice.Viewport.Width;
            Vector2 CameraCentre = new Vector2(GraphicsWidth / 2, GraphicsHeight / 2);
            TextBox MousePosTextBox = TextBox.ListOfTextBoxes.Find(x => x.Name == "MousePos");
            if (MousePosTextBox != null)
            {
                MousePos.X = MathUtil.SimplifyDouble(MousePos.X, 0);
                MousePos.Y = MathUtil.SimplifyDouble(MousePos.Y, 0);
                MousePosTextBox.Text = "Mouse Pos: " + (MousePos).ToString(); // World Pos
            }
            TextBox WorldMousePosTextBox = TextBox.ListOfTextBoxes.Find(x => x.Name == "WorldMousePos");
            if (WorldMousePosTextBox != null)
            {
                //(This is excluding the camera pos movement)
                // V = (A-CP)*Zoom + CP
                // V = A*Zoom - CP*(Zoom - 1)
                //A = (CP*(Zoom - 1) +V) / Zoom

                //V = relative pos(in here, its mouse relative pos), cp = centre pos, a = mouse world pos, zoom = zoom factor
                Vector2 WorldMousePos = (CameraCentre * (Visualiser.Zoom - 1) + MousePos) / Visualiser.Zoom + CameraPos;
                WorldMousePos.X = MathUtil.SimplifyDouble(WorldMousePos.X, 0);
                WorldMousePos.Y = MathUtil.SimplifyDouble(WorldMousePos.Y, 0);
                WorldMousePosTextBox.Text = "World Mouse Pos: " + (WorldMousePos).ToString(); // World Pos
            }
            //Detail orientation of tracked boids.
            UIList BoidsBeingViewedList = UIList.ListOfUILists.Find(x => x.Name == "BoidsBeingViewedList");
            if (BoidsBeingViewedList != null)
            {
                for (int i = 0; i < BoidsBeingTracked.Count; i++)
                {
                    Entity TrackedBoid = BoidsBeingTracked[i];
                    int BC = BoidsBeingViewedList.Contents.Count;
                    UIElement element;
                    if (i >= 0 && i < BC)
                    {
                        element = BoidsBeingViewedList.Contents[i];
                    }
                    else
                    {
                       
                        element = new TextBox(Vector2.Zero, null, "a", GeneralUtil.RandomID());
                        BoidsBeingViewedList.Contents.Add(element);
                    }
                    if (element is TextBox textBox)
                    {
                        float TheBoidOrientation = MathUtil.SimplifyDouble(180 / (float)Math.PI * MathUtil.NormaliseAngle(TrackedBoid.Orientation), 3);
                        float AngleToTarget = MathUtil.SimplifyDouble(180 / (float)Math.PI * MathUtil.NormaliseAngle(TrackedBoid.Properties.AngleToTarget), 3);

                        textBox.Text = TheBoidOrientation.ToString() + ",  " +
                             (AngleToTarget).ToString();
                    }

                }
            }

            // TODO: Add your update logic here
            EntityFunctions.Update(gameTime);
            GeneralUtil.DeferredCollectionManager<UIElement>.ApplyDeferred();
            Player.Update(gameTime, GameClient);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Visualiser.Draw(_spriteBatch);
            base.Draw(gameTime);
            
        }
    }
}
