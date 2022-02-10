using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Entities;

namespace MyGame.MyGame;


public class MyGame : Game
{
	public const bool DEBUG_MODE = true;

	private MyGame() : base(1920, 1080, false, false, pPixelArt: true)
	{
		targetFps = 60;
		// Draw some things on a canvas:
		EasyDraw canvas = new(width, height);
		canvas.Clear(Color.MediumPurple);
		canvas.Fill(Color.Yellow);
		canvas.Ellipse(width / 2.0f, height / 2.0f, 200, 200);
		canvas.Fill(50);
		canvas.TextSize(32);
		canvas.TextAlign(CenterMode.Center, CenterMode.Center);
		canvas.Text("Welcome!", width / 2.0f, height / 2.0f);

		// Add the canvas to the engine to display it:
		AddChild(canvas);

		AddChild(new Enemy(new Vector2(width/2.0f, 0.0f)));
		AddChild(new Player(new Vector2(0.0f, 0.0f)));


		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	private void Update()
	{
		//Empty
	}

	private static void Main()
	{
		new MyGame().Start();
	}
}
