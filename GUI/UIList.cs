using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boid_Simulator.GUI
{
    internal class UIList
    {
        public string Name;
        public Vector2 StartingPosition;
        public static List<UIList> ListOfUILists = new();
        public List<UIElement> Contents = new();
        public UIList(Vector2 startingPosition, string name = "UIList")
        {
            StartingPosition = startingPosition;
            this.Name = name;
        }
    }
}
