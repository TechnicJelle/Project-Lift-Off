using System;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Drone : Enemy
{
	private const float MOVEMENT_SPEED = 0.5f;
	private const float EXPLOSION_RANGE = 100.0f;
	private const float WALL_ASSIST = 0.2f;

	public Drone(TiledObject obj) : base("enemyDrone.png", 12, 2, 24, 1, obj)
	{
		SetCycle(0, 11, 50);
		AnimateFixed();
		ApplyGravity = false;
	}

	protected new void Update()
	{
		if (!base.Update()) return;
		if (!this.visible || MyGame.LevelManager.CurrentLevel().Player == null) return;
    
		Vector2 toTarget = Vector2.Sub(MyGame.LevelManager.CurrentLevel().Player.GetPos(), new Vector2(x, y));
		ApplyForce(Vector2.SetMag(toTarget, MOVEMENT_SPEED));

		if (toTarget.MagSq() < EXPLOSION_RANGE * EXPLOSION_RANGE)
			Explode();
	}

	protected override void CollidedWithSide(Vector2 normal)
	{
		_vel.y -= WALL_ASSIST;
		base.CollidedWithSide(normal);
	}

	private void Explode()
	{
		MyGame.LevelManager.CurrentLevel().Player.TakeDamage(_vel);
		TakeDamage(new Vector2(0, 0));
		game.AddChild(new DroneExplosion(GetPos()));
	}
}
