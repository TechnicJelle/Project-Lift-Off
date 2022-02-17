using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private const byte ANIMATION_DELAY = 200;

	protected Enemy(TiledObject obj, int health) : base("evilBarry.png", 4, 2, 7, health, ANIMATION_DELAY, true)
	{
	}

	public void Bonk()
	{
		Console.WriteLine(this + " bonque " + Utils.Random(0, 10));
	}
}
