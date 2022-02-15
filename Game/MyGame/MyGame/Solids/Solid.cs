using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Solids;

public class Solid : AnimationSprite
{
	protected Solid(TiledObject obj) : this(new Vector2(obj.X, obj.Y), new Vector2(obj.Width, obj.Height))
	{

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
			MyGame.DebugCanvas.Ellipse(GetPos().x, GetPos().y, 16, 16);
		}
	}
}
