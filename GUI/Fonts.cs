using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.GUI
{
    internal class Fonts
    {
        public Dictionary<string, SpriteFont> FontDictionary = new();
        public Fonts()
        {
            FontDictionary["Montserrat"] = Game1.Instance.Content.Load<SpriteFont>("Fonts/Montserrat");
        }
    }
}
