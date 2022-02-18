using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private readonly int _millisAtSpawn;

	protected Enemy(string filename, int cols, int rows, int frames, int health, TiledObject obj) : base(filename, cols, rows, frames, health)
	{
		// Console.WriteLine(obj.Name);
		// if ()
		_millisAtSpawn = int.Parse(obj.Name);
	}

	protected new bool Update()
	{
		if (Time.time - MyGame.LevelManager.CurrentLevel().MillisAtStart > _millisAtSpawn)
		{
			this.visible = true;
			base.Update();
		}
		else
		{
			this.visible = false;
		}

		return visible;
	}
}
