using System;
using GXPEngine;

namespace MyGame.MyGame;

// ReSharper disable once InconsistentNaming
public static class UI
{
	private const string HEART_IMAGE = "circle.png";

	private static int _playerHealth = MyGame.PLAYER_HEALTH;

	public static EasyDraw Canvas;
	private static Sprite[] _hearts;

	public static void Init()
	{
		Canvas = new EasyDraw(Game.main.width, Game.main.height);
		Game.main.AddChild(Canvas);

		Sprite heartImage = new(HEART_IMAGE);

		_hearts = new Sprite[MyGame.PLAYER_HEALTH];
		for (int i = 0; i < _hearts.Length; i++)
		{
			Game.main.AddChild(_hearts[i] = new Sprite(HEART_IMAGE, false, false)
			{
				x = 300 + i * (heartImage.width + 30),
				y = 30,
			});
		}
	}

	public static void Update()
	{
		Canvas.ClearTransparent();

		Canvas.Text("Wave: " +  MyGame.Level._currentWave + "/" + MyGame.Level._totalWaves, Game.main.width - 100, 200); //TODO: Designer
	}

	public static void ReduceHearts(int amount = 1)
	{
		_playerHealth -= amount;
		for (int i = 0; i < _hearts.Length; i++)
		{
			_hearts[i].visible = i < _playerHealth;
		}
	}
}
