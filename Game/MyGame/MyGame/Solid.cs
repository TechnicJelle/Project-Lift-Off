using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame;

public class Solid : AnimationSprite
{
	// ReSharper disable once MemberCanBeProtected.Global
	public Solid(TiledObject obj) : this(new Vector2(obj.X, obj.Y), new Vector2(obj.Width, obj.Height), false)
	{
		const float tolerance = 0.1f;
		float w = obj.Width;
		float h = obj.Height;
		string filename = "barry";
		if (Math.Abs(w - 128) < tolerance && Math.Abs(h - 128) < tolerance)
		{
			filename = Utils.Random(0, 100) < 50 ? "box2x2L" : "box2x2R";
		}
		else if (Math.Abs(w - 256) < tolerance && Math.Abs(h - 64) < tolerance)
		{
			filename = "platform4x1";
		}
		else if (Math.Abs(w - 1408) < tolerance && Math.Abs(h - 64) < tolerance)
		{
			filename = "platform22x1";
		}
		else if (Math.Abs(w - 1024) < tolerance && Math.Abs(h - 64) < tolerance)
		{
			filename = "platform16x1";
		}
		else if (Math.Abs(w - 128) < tolerance && Math.Abs(h - 64) < tolerance)
		{
			filename = "platform3x1";
		}
		else if (Math.Abs(w - 192) < tolerance && Math.Abs(h - 64) < tolerance)
		{
			filename = "platform3x1";
		}
		else if (Math.Abs(w - 256) < tolerance && Math.Abs(h - 128) < tolerance)
		{
			filename = "box1x1Mid";
		}
		else
		{
			Console.WriteLine(this + ": " + obj.Width + ", " + obj.Height);
		}


		initializeFromTexture(Texture2D.GetInstance(filename + ".png"));
	}

	public Solid(Vector2 p, Vector2 s, bool addToGame = true) : base("colors.png", 1, 1)
	{
		x = p.x;
		y = p.y;
		// ReSharper disable once VirtualMemberCallInConstructor
		width = (int) s.x;
		// ReSharper disable once VirtualMemberCallInConstructor
		height = (int) s.y;

		if(addToGame)
			game.AddChild(this);
	}

	protected Solid(string filename, int cols, int rows, int frames,
		bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		game.AddChild(this);
	}

	/// <summary>
	/// Gets the coordinates of the Top-Left of the object.
	/// </summary>
	/// <returns>Vector2 with X and Y coordinates</returns>
	public Vector2 GetPos()
	{
		return new Vector2(x - width/2.0f, y - height/2.0f);
	}

	public Vector2 GetSize()
	{
		return new Vector2(width, height);
	}

	protected void Update()
	{
		if (MyGame.DEBUG_MODE)
		{
			MyGame.DebugCanvas.ShapeAlign(CenterMode.Min, CenterMode.Min);
			MyGame.DebugCanvas.NoFill();
			MyGame.DebugCanvas.Stroke(255, 0, 0);
			MyGame.DebugCanvas.StrokeWeight(3);
			MyGame.DebugCanvas.Rect(GetPos().x, GetPos().y, width, height);
			MyGame.DebugCanvas.Fill(255, 0, 0);
			MyGame.DebugCanvas.NoStroke();
			MyGame.DebugCanvas.ShapeAlign(CenterMode.Center, CenterMode.Center);
			MyGame.DebugCanvas.Ellipse(GetPos().x, GetPos().y, 16, 16);
		}
	}
}
