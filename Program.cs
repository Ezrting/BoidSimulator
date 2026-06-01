
using Boid_Simulator;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using System.Diagnostics;
using Boid_Simulator.GUI;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


using var game = new Boid_Simulator.Game1();

Thread GameThread = new Thread(() =>
{
    float SideLength = 100; //Only the side length of the bottom, shortest side as the Boid is an isosceles
    Vector2 BoidOrigin = new Vector2(400, 0);
    Line Line1 = new(new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2));
    Line Line2 = new(new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength));
    Line Line3 = new(new Vector2(0, SideLength), new Vector2(-SideLength / 2, -SideLength / 2));
    Line Line4 = new(BoidOrigin, BoidOrigin);
    Line[] LineList = new Line[] { Line1, Line2, Line3};
    float Direction = 10 * (float)Math.PI / 180f; // in radians
    Vector2[] PositionList = new Vector2[] { new Vector2(-SideLength / 2, -SideLength / 2), new Vector2(SideLength / 2, -SideLength / 2), new Vector2(0, SideLength) };
    float cos = (float)Math.Cos(Direction);
    float sin = (float)Math.Sin(Direction);
    for (int i = 0; i < PositionList.Length; i++)
    {
        PositionList[i] = new Vector2(cos * PositionList[i].X - sin * PositionList[i].Y, sin * PositionList[i].X + cos * PositionList[i].Y);
        //I used AI
    }
    for (int i = 0; i < PositionList.Length; i++)
    {
        //Line1.StartPos = new Vector2((float)Math.Cos(Direction) * line.StartPos.X, (float)Math.Sin(Direction) * line.StartPos.Y);
        // Line1.EndPos = new Vector2((float)Math.Cos(Direction) * line.EndPos.X, (float)Math.Sin(Direction) * line.EndPos.Y);
        PositionList[i] += BoidOrigin;
    }
    Shape shape = new Shape(PositionList);
    Game1.CameraCentre = new Vector2(-300, -200);
    //Create a text box
    TextBox textBox = new TextBox(new Vector2(10, 10), "Hello World");
}
);
GameThread.Start();


game.Run();






