using System;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Turret : Enemy
{
	private const int MILLIS_BETWEEN_SHOTS = 1500;

	private int millisAtLastShot;

	public Turret(TiledObject obj) : base("enemyShooter.png", 18, 3, 18, 1, obj)
	{
		ApplyGravity = false;
	}

	protected new void Update()
	{
		if(!base.Update()) return;
		if (Time.time - millisAtLastShot <= MILLIS_BETWEEN_SHOTS) return;
		Shoot();
		millisAtLastShot = Time.time;
	}

	private void Shoot()
	{
		Console.WriteLine(this + "pew!");
		SoundManager.shooter.Play();
	}
}
