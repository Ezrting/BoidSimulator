using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Boid_Simulator.GUI
{
    internal class TextButton : TextBox
    {
        bool IsBeingHeld = false;
        public TextButton(Vector2 screenPosition, string text, string name) : base(screenPosition, text, name)
        {
        }
    }
}
