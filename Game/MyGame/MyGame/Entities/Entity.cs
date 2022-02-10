using System;
using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Solids;

namespace MyGame.MyGame.Entities;

public class Entity : AnimationSprite
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private readonly Vector2 _gravity = new(0.0f, 2.5f);

	//Physics variables
	private readonly Vector2 _vel;                                                                                                                                                                                                                                           
	private readonly Vector2 _acc;

	//Collision variables
	protected bool Colliding;

	protected Entity(string filename, int cols, int rows, int frames,
		byte animationDelay = 1, bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		SetAnimationDelay(animationDelay);
		_vel = new Vector2(0.0f, 0.0f);
		_acc = new Vector2(0.0f, 0.0f);
		
		this.collider.isTrigger = true;
		game.AddChild(this);
	}

	private void SetAnimationDelay(byte animationDelay)
	{
		SetCycle(0, _frames, animationDelay);
	}

	protected void ApplyForce(Vector2 f)
	{
		_acc.Add(f);
	}

	public void Collisions()
	{
		GameObject[] objects = this.GetCollisions(true, false);

		Colliding = false;
		foreach (GameObject obj in objects)
		{
			if (obj is Solid sol)
			{
				if (sol is Floor flo)
				{
					this.y = flo.y - height;
					this._vel.y = 0;
					this.Colliding = true;
				} else if (sol is Roof roo)
				{
					
				} else if (sol is Wall wal)
				{
					this.x = wal.x - width;
					this._vel.x = 0;
					this.Colliding = true;
				} else if (sol is Platform pla)
				{
					this.y = pla.y - height;
					this._vel.y = 0;
					this.Colliding = true;
				}
			} else if (obj is Entity ant)
			{
				
			}
		}
	}

	protected void Update()
	{

		Animate(Time.deltaTime);
		ApplyForce(_gravity);

		_vel.Add(_acc);
		_vel.Mult(DRAG_MULTIPLIER);
		x += _vel.x;
		y += _vel.y;
		Collisions();
			
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
		// if (_pos.y > Game.main.height - height)
		// {
		// 	_pos.y = Game.main.height - height;
		// 	_vel.y = 0;
		// 	Colliding = true;
		// }
		// else
		// {
		// 	Colliding = false;
		// }
	}
}
