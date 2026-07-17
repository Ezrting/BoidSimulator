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
            TextBox MousePosTextBox = TextBox.ListOfTextBoxes.Find(x => x.Name == "MousePos");
            if (MousePosTextBox != null)
            {
                MousePosTextBox.Text = "Mouse Pos: " + (Mouse.GetState().Position.ToVector2()).ToString(); // World Pos
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
                        element = new TextBox(Vector2.Zero, "a", GeneralUtil.RandomID());
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
