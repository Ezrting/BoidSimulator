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
    public class TextBox : UIElement
    {
        public static List<TextBox> ListOfTextBoxes = new();
        public static Dictionary<Vector2, List<TextBox>> ListOfTextBoxGroups = new(){
            { new Vector2 (0,0), new List<TextBox>() }
        };
        public string Text;
        public float FontSize = 12;
        public Color BoxColour = Color.Red;
        public float BoxTransparency = 1;
        public TextBox(Vector2 screenPosition, string text, string name) : base(screenPosition, name) //text is not part of base class
            //Take the first and second parameters of UIElement and assign them to the corresponding names.
        {
            this.ScreenPosition = screenPosition;
            this.Text = text;
            this.Name = name;
            ListOfTextBoxes.Add(this);
            if (!ListOfTextBoxGroups.TryGetValue(screenPosition, out var Value)) //The box position is only the starting position of the list of textboxes.
            {
                //If no list for the screen position was created yet
                ListOfTextBoxGroups[screenPosition] = [this]; // Simplified
            }
            else if (Value != null)
            {
                Value.Add(this);
            }
        }
        public static TextBox StateByTextBox(string BoxText, Vector2 BoxPosition, string BoxName)
        {

            TextBox Found = ListOfTextBoxes.Find(x => x.Name == BoxName);
            if (Found != null)
            {
                Found.Text = BoxText;
                Found.ScreenPosition = BoxPosition;
                return Found;
            }
            else
            {

                TextBox New = new TextBox(BoxPosition, BoxText, BoxName);
                return New;
            }
        }

    }
}
