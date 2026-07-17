using System;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Boid_Simulator.GUI
{

    public abstract class UIElement
    {
        public static List<UIElement> ListOfUIElements = new();
        public Vector2 ScreenPosition; //Screen position is overrided in lists
        public int LayerIndex;
        public string Name;
        public Color BoxColour = Color.Red;
        public float BoxTransparency = 1;
        public Shape BoxShape;
        public UIElement(Vector2 screenPosition, string name)
        {
            this.ScreenPosition = screenPosition;
            this.Name = name;
            ListOfUIElements.Add(this);
        }
    }
}
