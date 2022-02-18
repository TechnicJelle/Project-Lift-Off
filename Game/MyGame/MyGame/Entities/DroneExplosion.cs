using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class DroneExplosion : AnimationSprite
{
	public DroneExplosion(Vector2 spawnPos) : base("enemyDrone.png", 12, 2, 24, true, false)
	{
		x = spawnPos.x;
		y = spawnPos.y;
		SetCycle(12, 12, 5);
	}

	public void Update()
	{
		AnimateFixed();
		if(currentFrame >= 23)
			game.RemoveChild(this);
	}
}
