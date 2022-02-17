using System;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private const byte ANIMATION_DELAY = 200;

	public Enemy(Vector2 spawnPos) :
		base("evilBarry.png", 4, 2, 7, ANIMATION_DELAY, true)
	{
		x = spawnPos.x;
		y = spawnPos.y;
	}

	public Enemy(TiledObject obj) : base("evilBarry.png", 4, 2, 7, ANIMATION_DELAY, true)
	{
	}

	public void Bonk()
	{
		Console.WriteLine("bonque");
	}
}
