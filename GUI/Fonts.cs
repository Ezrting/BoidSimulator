using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.GUI
{
    internal static class Fonts
    {
        public static Dictionary<string, SpriteFont> FontDictionary = new()
        {
            ["Montserrat"] = GameIns.Instance.Content.Load<SpriteFont>("Fonts/Montserrat")
        };
    }
}
