
using Boid_Simulator;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using System.Diagnostics;


using var game = new Boid_Simulator.Game1();
Thread GameThread = new Thread(() =>
{
    float SideLength = 20;
    Line Line1 = new(new Vector2(0, 0), new Vector2(SideLength, 0));
    Line Line2 = new(new Vector2(SideLength / 2, SideLength * 2), new Vector2(0, 0));
    Line Line3 = new(new Vector2(SideLength, 0), new Vector2(SideLength / 2, SideLength * 2));
    Shape shape = new Shape(new Line[] { Line1, Line2, Line3 });
    Game1.CameraCentre = new Vector2(-400, -400);
}
);
GameThread.Start();


game.Run();

