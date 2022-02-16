using System;
using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Solids;

namespace MyGame.MyGame.Entities;

public class Entity : Solid
{
	//Variables for the designers:
	private const float DRAG_MULTIPLIER = 0.9f;
	private const float WALL_SLIDE_DRAG_MULTIPLIER = 0.5f;
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
		// if (this.GetType() == typeof(Player)) Console.WriteLine("+acc: " + _acc);
	}

	protected new void Update()
	{
		Animate(Time.deltaTime);
		ApplyForce(_gravity);


		_vel.Add(_acc);
		// if(this.GetType() == typeof(Player)) Console.WriteLine("facc: " + _acc);
		_vel.Mult(DRAG_MULTIPLIER);

		if (this.GetType() == typeof(Player)) Console.WriteLine("bvel: " + _vel); //Before the collision check, the y velocity is good.

		//Collision calculations
		CollidingWithFloor = false;
		foreach (Solid solidInLevel in MyGame.Level.Solids)
		{
			if (this == solidInLevel) continue; //don't collide with yourself!!!!!!
			Collision c = Collision.DynamicRectVsRect(this, solidInLevel);
			if (c.Result)
			{
				// if (this.GetType() == typeof(Player)) Console.WriteLine(this + ": collision");
				_vel.Add(Vector2.Mult(c.ContactNormal, new Vector2(Mathf.Abs(_vel.x), Mathf.Abs(_vel.y)))); //TODO: fix this y component always being made 0

				// MyGame.DebugCanvas.Stroke(100, 100, 255);
				// MyGame.DebugCanvas.StrokeWeight(2);
				// MyGame.DebugCanvas.Line(c.ContactPoint.x, c.ContactPoint.y, c.ContactPoint.x + 100 * c.ContactNormal.x, c.ContactPoint.y + 100 * c.ContactNormal.y);
				CollidingWithFloor = true;
			}
		}

		if (this.GetType() == typeof(Player)) Console.WriteLine("avel: " + _vel); //After the collision check, the y velocity has been reset to 0.
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
