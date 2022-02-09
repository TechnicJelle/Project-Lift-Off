using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame;

public class Entity : AnimationSprite
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private readonly Vector2 gravity = new(0.0f, 2.5f);

	//Physics variables
	private readonly Vector2 pos;
	private readonly Vector2 vel;
	private readonly Vector2 acc;

	//Collision variables
	protected bool colliding;

	protected Entity(Vector2 spawnPos, string filename, int cols, int rows, int frames,
		byte animationDelay = 1, bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		SetCycle(0, frames, animationDelay);
		pos = spawnPos.Copy();
		vel = new Vector2(0.0f, 0.0f);
		acc = new Vector2(0.0f, 0.0f);
	}

	protected void ApplyForce(Vector2 f)
	{
		acc.Add(f);
	}

	protected void Update()
	{
		Animate(Time.deltaTime);
		ApplyForce(gravity);

		vel.Add(acc);
		vel.Mult(DRAG_MULTIPLIER);
		pos.Add(vel);
		_mirrorX = acc.x switch
		{
			//Mirror the sprite based on the acceleration direction
			//Using velocity would be more realistic, but using the acceleration feels better
			< 0 => true,
			> 0 => false,
			_ => _mirrorX,
		};
		acc.Mult(0.0f);

		//Temporary code to make the player not fall out of the map.
		//TODO: Replace this with actual tiles and collision
		if (pos.y > Game.main.height - height)
		{
			pos.y = Game.main.height - height;
			vel.y = 0;
			colliding = true;
		}
		else
		{
			colliding = false;
		}

		x = pos.x;
		y = pos.y;
	}
}
