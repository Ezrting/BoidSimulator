using Boid_Simulator.Graphics;
using Boid_Simulator.GUI;
using Boid_Simulator.EntityHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator
{
    public class Player
    {
        public GameIns ControlledGame;
        public Entity ControlledCharacter;
        public static Keys[] CurrentlyPressedKeys = new Keys[0];
        
        
        public Player(GameIns controlledGame)
        {
            this.ControlledGame = controlledGame;
        }
        static public void Update(GameTime gameTime, Player plr)
        {
            double DeltaTime = gameTime.TotalGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            CurrentlyPressedKeys = keyboardState.GetPressedKeys();

            if (plr.ControlledCharacter == null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    
                }
                if (keyboardState.IsKeyDown(Keys.M))
                {
                    Visualiser.Zoom *= 0.99f;
                    TextBox.StateByTextBox("Zoom:" + Visualiser.Zoom, new Vector2(1700, 10), "ZoomStater");

                }
                if (keyboardState.IsKeyDown(Keys.N))
                {
                    Visualiser.Zoom *= 1.01f;
                    TextBox.StateByTextBox("Zoom:" + Visualiser.Zoom, new Vector2(1700, 10), "ZoomStater");
                }
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    GameIns.CameraPos.Y -= 5f / Visualiser.Zoom;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    GameIns.CameraPos.X -= 5f / Visualiser.Zoom;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    GameIns.CameraPos.Y += 5f / Visualiser.Zoom;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    GameIns.CameraPos.X += 5f / Visualiser.Zoom;
                }
            }
            else
            {
            }

        }
    }
}
