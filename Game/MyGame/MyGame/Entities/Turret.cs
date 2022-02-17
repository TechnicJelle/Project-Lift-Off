using System;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Turret : Enemy
{
	public Turret(TiledObject obj) : base("enemyShooter.png", 18, 3, 18, 1, obj)
	{
		ApplyGravity = false;
	}

	protected new void Update()
	{
		if(!base.Update()) return;
		if(frameCount % 300 == 0)
			Console.WriteLine(this + "pew!");
	}
}
