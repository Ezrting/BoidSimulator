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
        public static Fonts GraphicalFonts;
        public Fonts()
        {
            FontDictionary["Montserrat"] = GameIns.Instance.Content.Load<SpriteFont>("Fonts/Montserrat");
        }
    }
}
