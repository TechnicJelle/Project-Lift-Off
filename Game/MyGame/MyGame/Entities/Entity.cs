using System;
using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Solids;

namespace MyGame.MyGame.Entities;

public class Entity : AnimationSprite
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private const float WALL_SLIDE_DRAG_MULTIPLIER = 0.5f;
	private readonly Vector2 _gravity = new(0.0f, 2.5f);

	//Physics variables
	private readonly Vector2 _vel;
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

		collider.isTrigger = true;
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

	private void Collisions()
	{
		GameObject[] objects = GetCollisions(true, false);

		CollidingWithFloor = false;
		foreach (GameObject obj in objects)
		{
			if (obj.GetType() == typeof(Sprite)) continue;
			Sprite spr = (Sprite) obj;
			if (MyGame.DEBUG_MODE)
			{
				MyGame.DebugCanvas.Fill(255);
				MyGame.DebugCanvas.Rect(spr.x, spr.y, spr.width, spr.height);
				MyGame.DebugCanvas.Fill(255, 0, 0);
				MyGame.DebugCanvas.Ellipse(spr.x, spr.y, 16, 16);
			}
			switch (spr)
			{
				case Floor flo:
					y = flo.y - height;
					_vel.y = 0;
					CollidingWithFloor = true;
					break;
				case Roof roo:
					y = roo.y + roo.height;
					_vel.y = 0;
					if (GetType() == typeof(Player))
					{
						Player plr = (Player) this;
						plr.StopJump();
					}
					break;
				case Wall wal:
					if (_vel.x < 0) //hit left wall
						x = wal.x + wal.width;
					else //hit right wall
						x = wal.x - width;
					_vel.x = 0;
					CollidingWithFloor = true;
					_vel.y *= WALL_SLIDE_DRAG_MULTIPLIER;
					break;
				case Platform pla:
					CollidingWithFloor = true;


					switch (SideHit(pla))
					{
						case 1:
							x = pla.x - pla.width / 2f - width / 2f - 1;
							_vel.x = 0;
							break;

						case 3:
							x = pla.x + pla.width / 2f + width / 2f + 1;
							_vel.x = 0;
							break;

						case 2:
							y = pla.y - pla.height / 2f - height / 2f;
							_vel.y = 0;
							break;
						case 4:
							y = pla.y + pla.height / 2f + height / 2f;
							_vel.y = 0;
							if (GetType() == typeof(Player))
							{
								Player plr = (Player) this;
								plr.StopJump();
								CollidingWithFloor = false;
							}
							break;
					}

					break;
				case Solid sol:
					throw new Exception("The game should not have any objects of type Solid. Only use types that extend it. " + sol);
					break;

				case Entity ant:
					break;
			}
		}
	}

	/// <summary>
	/// 0: nothing<br/>
	/// 1: left<br/>
	/// 2: top<br/>
	/// 3: right<br/>
	/// 4: bottom
	/// </summary>
	/// <param name="pla"></param>
	/// <returns></returns>
	private int SideHit(Platform pla)
	{
		bool ySide = y < pla.y+5;
		if (ySide && y + height / 2f > pla.y - pla.height / 2f)
		{
			return 2;
		}

		if (!ySide && y - height / 2f < pla.y + pla.height / 2f)
		{
			return 4;
		}

		// bool xSide = x < pla.x;
		// if (xSide && x + width / 2f > pla.x - pla.width / 2f)
		// {
		// 	return 1;
		// }
		//
		// if (!xSide && x - width / 2f < pla.x + pla.width / 2f)
		// {
		// 	return 3;
		// }

		return 0;
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
