using GXPEngine;

namespace MyGame.MyGame;

// ReSharper disable once InconsistentNaming
public static class UI
{
	private static EasyDraw _canvas;

	public static void Init()
	{
		_canvas = new EasyDraw(Game.main.width, Game.main.height);
		Game.main.AddChild(_canvas);
	}

	public static void Update()
	{
		_canvas.Ellipse(100,100, 50, 70);
	}
}
