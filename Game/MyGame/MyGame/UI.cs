using GXPEngine;

namespace MyGame.MyGame;

// ReSharper disable once InconsistentNaming
public static class UI
{
	private const string HEART_IMAGE = "heart.png";

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
				x = 10 + i * (heartImage.width + 30),
				y = 10,
			});
		}
	}

	public static void Update()
	{
		Canvas.ClearTransparent();

		Text("Wave: " +  MyGame.LevelManager.CurrentLevel()._currentWave + "/" + MyGame.LevelManager.CurrentLevel()._totalWaves, Game.main.width, 32);
	}

	public static void ReduceHearts(int amount = 1)
	{
		_playerHealth -= amount;
		for (int i = 0; i < _hearts.Length; i++)
		{
			_hearts[i].visible = i < _playerHealth;
		}
	}

	public static void Text(string str, float x, float y)
	{
		Canvas.TextAlign(CenterMode.Max, CenterMode.Min);
		Canvas.Fill(0, 150);
		Canvas.Text(str, x+1, y+1);
		Canvas.Fill(255);
		Canvas.Text(str, x, y);
	}
}
