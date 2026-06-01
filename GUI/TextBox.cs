using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.GUI
{
    
    public class TextBox
    {
        public static List<TextBox> ListOfTextBoxes = new();
        public Vector2 ScreenPosition;
        public string Text;
        public TextBox(Vector2 screenPosition, string text)
        {
            this.ScreenPosition = screenPosition;
            this.Text = text;
            ListOfTextBoxes.Add(this);
        }
    }
}
