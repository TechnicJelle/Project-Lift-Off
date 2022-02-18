using System;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class WalkerExplosion : AnimationSprite
{
	public WalkerExplosion(Vector2 spawnPos, int w, int h) : base("enemyRobot.png", 8, 3, 24, true, false)
	{
		x = spawnPos.x;
		y = spawnPos.y;
		width = w;
		height = h;
		SetCycle(16, 12, 7);
	}

	public void Update()
	{
		AnimateFixed();
		if(currentFrame >= 23)
			game.RemoveChild(this);
	}
}
