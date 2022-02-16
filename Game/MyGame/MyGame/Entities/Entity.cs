using System;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class Entity : Solid
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private const float FLOOR_WALK_DRAG_MULTIPLIER = 0.9f;
	private const float WALL_SLIDE_DRAG_MULTIPLIER = 0.2f;
	private const float PLATFORM_CLIMB_ASSIST_MULTIPLIER = 1.4f;
	private readonly Vector2 _gravity = new(0.0f, 2.5f);

	//Physics variables
	public Vector2 _vel { get; }
	private readonly Vector2 _acc;

	//Collision variables
	protected bool CollidingWithFloor;

	protected Entity(string filename, int cols, int rows, int frames,
		byte animationDelay = 1, bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		SetAnimationDelay(animationDelay);
		_vel = new Vector2(0.0f, 0.0f);
		_acc = new Vector2(0.0f, 0.0f);
	}

	private void SetAnimationDelay(byte animationDelay)
	{
		SetCycle(0, _frames, animationDelay);
	}

	protected void ApplyForce(Vector2 f)
	{
		_acc.Add(f.Copy());
	}

	protected new void Update()
	{
		Animate(Time.deltaTime);
		ApplyForce(_gravity);


		_vel.Add(_acc);
		_vel.Mult(DRAG_MULTIPLIER);

		//Collision calculations
		CollidingWithFloor = false;
		foreach (Solid solidInLevel in MyGame.Level.Solids)
		{
			if (this == solidInLevel) continue; //don't collide with yourself!!!!!!

			(bool isColliding, Vector2 contactPoint, Vector2 contactNormal, float tHitNear) = Collision.DynamicRectVsRect(this, solidInLevel);
			if (!isColliding) continue; //if no collision
			if (GetType() == typeof(Player) && solidInLevel.GetType() == typeof(Enemy)) //if you're a player, don't collide with enemies
			{
				Player player = (Player) this;
				player.CurrentlyCollidingWithEnemies.Add((Enemy)solidInLevel);
				continue;
			}

			CollidingWithFloor = true;

			_vel.Add(Vector2.Mult(contactNormal,
				new Vector2(Mathf.Abs(_vel.x), Mathf.Abs(_vel.y))));
			switch (contactNormal.y)
			{
				case < -0.1f:
					_vel.x *= FLOOR_WALK_DRAG_MULTIPLIER;
					break;
				case > 0.1f:
					Player plr = (Player) this;
					plr.StopJump();
					CollidingWithFloor = false;
					break;
			}

			if (solidInLevel.GetType() == typeof(SolidClimbable))
			{
				if(Mathf.Abs(contactNormal.x) > 0.1f)
					_vel.y *= WALL_SLIDE_DRAG_MULTIPLIER;
			}
			else
			{
				if (_vel.y < 0)
				{
					_vel.y *= PLATFORM_CLIMB_ASSIST_MULTIPLIER;
				}
			}

			if (MyGame.DEBUG_MODE)
			{
				MyGame.DebugCanvas.Stroke(255, 255, 0);
				MyGame.DebugCanvas.StrokeWeight(2);
				MyGame.DebugCanvas.Line(contactPoint.x, contactPoint.y,
					contactPoint.x + 30 * contactNormal.x, contactPoint.y + 30 * contactNormal.y);
			}
		}

		x += _vel.x;
		y += _vel.y;

		_mirrorX = _acc.x switch
		{
			//Mirror the sprite based on the acceleration direction
			//Using velocity would be more realistic, but using the acceleration feels better
			< 0 => true,
			> 0 => false,
			_ => _mirrorX,
		};
		_acc.Mult(0.0f);


		//Draw debug things:
		base.Update();
	}
}
