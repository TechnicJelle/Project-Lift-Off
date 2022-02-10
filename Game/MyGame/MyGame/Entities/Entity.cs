using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class Entity : AnimationSprite
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private readonly Vector2 _gravity = new(0.0f, 2.5f);

	//Physics variables
	private readonly Vector2 _pos;
	private readonly Vector2 _vel;
	private readonly Vector2 _acc;

	//Collision variables
	protected bool Colliding;

	protected Entity(Vector2 spawnPos, string filename, int cols, int rows, int frames,
		byte animationDelay = 1, bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		SetCycle(0, frames, animationDelay);
		_pos = spawnPos.Copy();
		_vel = new Vector2(0.0f, 0.0f);
		_acc = new Vector2(0.0f, 0.0f);
	}

	protected void ApplyForce(Vector2 f)
	{
		_acc.Add(f);
	}

	protected void Update()
	{
		Animate(Time.deltaTime);
		ApplyForce(_gravity);

		_vel.Add(_acc);
		_vel.Mult(DRAG_MULTIPLIER);
		_pos.Add(_vel);
		_mirrorX = _acc.x switch
		{
			//Mirror the sprite based on the acceleration direction
			//Using velocity would be more realistic, but using the acceleration feels better
			< 0 => true,
			> 0 => false,
			_ => _mirrorX,
		};
		_acc.Mult(0.0f);

		//Temporary code to make the player not fall out of the map.
		//TODO: Replace this with actual tiles and collision
		if (_pos.y > Game.main.height - height)
		{
			_pos.y = Game.main.height - height;
			_vel.y = 0;
			Colliding = true;
		}
		else
		{
			Colliding = false;
		}

		x = _pos.x;
		y = _pos.y;
	}
}
