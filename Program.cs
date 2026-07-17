
using Boid_Simulator;
using Boid_Simulator.EntityHandler;
using Boid_Simulator.Graphics;
using Boid_Simulator.GUI;
using Boid_Simulator.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;


using var game = new GameIns();

Thread GameThread = new Thread(() =>
{

        Color[] ColourArray = {
    Color.Aqua,
    Color.DeepPink,
    Color.Red,
    Color.Blue,
    Color.Magenta,
    Color.Green,
    Color.Brown,
    Color.SaddleBrown,
    Color.Salmon,
    Color.SeaGreen,
    Color.Purple,
    Color.DarkSlateGray,
    Color.MonoGameOrange,
    Color.Chocolate,
    Color.Chartreuse,
    Color.Gold,
    Color.Beige,
};
    while (!game.Initialised)
    {
        Thread.Sleep(100);
    }
        GeneralUtil.SplitTextIntoRows(Fonts.GraphicalFonts.FontDictionary["Montserrat"], "Chaa", 12, 100);
        Entity NewEntity = new Entity("PlayerBoid", new Vector2(900, 500), 100, new() { DrawColour = Color.White }) { RotateSpeed = 500f, Speed = 300f };
        TextBox textBox = new TextBox(new Vector2(10, 10), "Hello World", "MousePos");
        TextBox textBox2 = new TextBox(new Vector2(10, 20), "MousePointPosition", "WorldMousePos");
        TextBox textBox3 = new TextBox(new Vector2(10, 40), "Boids being viewed:", "InfoBoid");
        //VisualisedLine V = new(new Vector2(100, 300), new Vector2(1400, 300));
        UIList BoidsBeingViewedList = new(new Vector2(10, 50), "BoidsBeingViewedList");
        UIList.ListOfUILists.Add(BoidsBeingViewedList);
        UIElement t = new TextBox(new Vector2(10, 20), "Press X to control playerr", "In3fo");
        UIElement t2 = new TextBox(new Vector2(10, 20), "Press X to control playerrr", "In3fo");
        UIElement t4 = new TextBox(new Vector2(10, 20), "Press X to control playerrrr", "In3fo");
        BoidsBeingViewedList.Contents.Add(t);
        BoidsBeingViewedList.Contents.Add(t2);
        BoidsBeingViewedList.Contents.Add(t4);
        // V.Width = 50f;
        const double ArbitraryConstant = 2;
        for (int i = 0; i < 400; i++)
        {
            Random random = new();
            int RandomIndex = random.Next(ColourArray.Length);
            float RandomPosX = (float)random.NextDouble() * 1850 * (float)ArbitraryConstant;
            float RandomPosY = (float)random.NextDouble() * 1000 * (float)ArbitraryConstant;
            float rotateSpeed = (float)random.NextDouble() * 7f;
            float speed = 1100f + (float)random.NextDouble() * 600f;
            float sideLength = 60 + (float)random.NextDouble() * 60f;
            float StartingOrientation = (float)random.NextDouble() * (float)Math.PI * 2;
            Color RandomColour = ColourArray[RandomIndex];
            Entity NewEntity2 = new Entity("RegularBoid", new Vector2(RandomPosX, RandomPosY), 20, new() { DrawColour = RandomColour }) { Orientation = StartingOrientation, RotateSpeed = rotateSpeed, Speed = speed, SideLength = sideLength };
            NewEntity2.PrimaryShape.Transparency = 0.2f;
            if (i == 32)
            {
                NewEntity2.SideLength *= 1;
                GameIns.BoidsBeingTracked.Add(NewEntity2);
            }
        }

}
);
GameThread.Start();


game.Run();






