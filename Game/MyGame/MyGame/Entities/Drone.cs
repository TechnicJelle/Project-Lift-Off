using System;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Drone : Enemy
{
	private const float MOVEMENT_SPEED = 0.5f;
	private const float EXPLOSION_RANGE = 200.0f;

	private bool _exploding = false;

	public Drone(TiledObject obj) : base("enemyDrone.png", 12, 2, 11, 1)
	{
		ApplyGravity = false;
	}

	protected new void Update()
	{
		Vector2 toTarget = Vector2.Sub(MyGame.LevelManager.CurrentLevel().Player.GetPos(), new Vector2(x, y));
		ApplyForce(Vector2.SetMag(toTarget, MOVEMENT_SPEED));
		if (_exploding)
		{
			if (_currentFrame >= 23) TakeDamage(new Vector2(0, 0));
			Console.WriteLine(_currentFrame);
		}
		else
		{
			base.Update();
		}
		if (toTarget.MagSq() < EXPLOSION_RANGE * EXPLOSION_RANGE)
			Explode();
	}

	private void Explode()
	{
		_exploding = true;
		MyGame.LevelManager.CurrentLevel().Player.TakeDamage(_vel);
		SetCycle(12, 12, 4);
		AnimateFixed();
		Console.WriteLine("explode");
	}
}
