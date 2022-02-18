using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Turret : Enemy
{
	private const int MILLIS_BETWEEN_SHOTS = 1500;

	private int _millisAtLastShot;

	public Turret(TiledObject obj) : base("enemyShooter.png", 18, 3, 18, 1, obj)
	{
		ApplyGravity = false;
	}

	protected new void Update()
	{
		if(!base.Update()) return;
		if (Time.time - _millisAtLastShot <= MILLIS_BETWEEN_SHOTS) return;
		Shoot();
		_millisAtLastShot = Time.time;
	}

	private void Shoot()
	{
		Console.WriteLine(this + "pew!");
		SoundManager.shooter.Play();
		Vector2 toTarget = Vector2.Sub(MyGame.LevelManager.CurrentLevel().Player.GetPos(), new Vector2(x, y));
		MyGame.LevelManager.CurrentLevel().AddSolid(new Bullet(x, y, toTarget));
	}
}
