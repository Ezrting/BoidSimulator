
using Boid_Simulator;
using Boid_Simulator.EntityHandler;
using Boid_Simulator.Graphics;
using Boid_Simulator.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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
    Entity NewEntity = new Entity("PlayerBoid", new Vector2(900, 500), 100, new() { DrawColour = Color.White }) { RotateSpeed = 500f, Speed = 300f };
    TextBox textBox = new TextBox(new Vector2(10, 10), "Hello World", "MousePos");
    TextBox textBox2 = new TextBox(new Vector2(10, 20), "Press X to control player", "Info");
    TextBox textBox3 = new TextBox(new Vector2(10, 30), "Boids being viewed:", "InfoBoid");
    //VisualisedLine V = new(new Vector2(100, 300), new Vector2(1400, 300));
    UIList BoidsBeingViewedList = new(new Vector2(10, 40), "BoidsBeingViewedList");
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
            NewEntity2.SideLength *= 10;
            GameIns.BoidsBeingTracked.Add(NewEntity2);
        }
    }
    //float SideLength = 100; //Only the side length of the bottom, shortest side as the Boid is an isosceles
    //Vector2 BoidOrigin = new Vector2(200, 400);
    //Line Line1 = new(new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2));
    //Line Line2 = new(new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength));
    //Line Line3 = new(new Vector2(0, SideLength), new Vector2(-SideLength / 2, -SideLength / 2));
    //Line Line4 = new(BoidOrigin, BoidOrigin);
    //Line[] LineList = new Line[] { Line1, Line2, Line3};
    //Vector2[] PositionList = new Vector2[] { new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength) };
    //GameIns.CameraPos = new Vector2(-0, -0);
    //Shape shape = new Shape(PositionList);
    ////Create a text box
    //for (int a = 0; a < 4000; a++)
    //{
    //    BoidOrigin += new Vector2(0.3f, 0);
    //    float Direction = 1 *a * (float)Math.PI / 180f; // in radians
    //    PositionList = new Vector2[] { new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength) };
    //    float cos = (float)Math.Cos(Direction);
    //    float sin = (float)Math.Sin(Direction);
    //    for (int i = 0; i < PositionList.Length; i++)
    //    {
    //        Vector2 Pivot = PositionList[i] - new Vector2(0, 0); // Negate the point minus the point you are turning around. Although note in this case PositionList[i] is relative to the centre right now, which
    //        //means the pivot is (0,0). This does make things easier but is important to keep in mind.
    //        PositionList[i] = new Vector2(cos * Pivot.X - sin * Pivot.Y, sin * Pivot.X + cos * Pivot.Y);
    //        //I used AI, but turns out this is the rotation matrix
    //    }
    //    for (int i = 0; i < PositionList.Length; i++)
    //    {
    //        //Line1.StartPos = new Vector2((float)Math.Cos(Direction) * line.StartPos.X, (float)Math.Sin(Direction) * line.StartPos.Y);
    //        // Line1.EndPos = new Vector2((float)Math.Cos(Direction) * line.EndPos.X, (float)Math.Sin(Direction) * line.EndPos.Y);
    //        PositionList[i] += BoidOrigin;
    //    }
    //    if (shape != null)
    //    {
    //        shape.CreateLines(PositionList);
    //    }

    //    Thread.Sleep(400);
    //}
}
);
GameThread.Start();


game.Run();






