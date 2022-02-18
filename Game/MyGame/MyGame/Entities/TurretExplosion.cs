using System;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class TurretExplosion : AnimationSprite
{
	public TurretExplosion(Vector2 spawnPos) : base("enemyShooter.png", 18, 3, 54, true, false)
	{
		x = spawnPos.x;
		y = spawnPos.y;
		SetCycle(36, 13, 7);
	}

	public void Update()
	{
		AnimateFixed();
		Console.WriteLine(currentFrame);
		if(currentFrame >= 45)
			game.RemoveChild(this);
	}
}
