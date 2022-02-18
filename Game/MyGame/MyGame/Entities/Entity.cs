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
	protected virtual int MILLIS_BETWEEN_DAMAGES() { return 500; }

	//Physics variables
	public Vector2 _vel { get; }
	private readonly Vector2 _acc;
	protected bool ApplyGravity = true;

	//Collision variables
	protected bool CollidingWithFloor;

	private int _health;
	private bool _invincible;
	private int _millisAtLastDamage;

	protected Entity(string filename, int cols, int rows, int frames, int health,
		byte animationDelay = 1, bool keepInCache = false, bool addCollider = true) :
		base(filename, cols, rows, frames, keepInCache, addCollider)
	{
		SetAnimationDelay(animationDelay);
		_vel = new Vector2(0.0f, 0.0f);
		_acc = new Vector2(0.0f, 0.0f);
		_health = health;
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
		if (Time.time - _millisAtLastDamage > MILLIS_BETWEEN_DAMAGES()) _invincible = false;
		Animate(Time.deltaTime);
		if (ApplyGravity) ApplyForce(_gravity);


		_vel.Add(_acc);
		_vel.Mult(DRAG_MULTIPLIER);

		//Collision calculations
		CollidingWithFloor = false;
		// Console.WriteLine(MyGame.LevelManager.CurrentLevel().Solids.Count);
		foreach (Solid solidInLevel in MyGame.LevelManager.CurrentLevel().GetVisibleSolids())
		{
			if (this == solidInLevel) continue; //don't collide with yourself!!!!!!

			(bool isColliding, Vector2 contactPoint, Vector2 contactNormal, float tHitNear) = Collision.DynamicRectVsRect(this, solidInLevel);

			if (!isColliding) continue; //if no collision
			if (!(this is Drone && solidInLevel is Drone)) //drones should collide with each other
			{
				if (this is Enemy && solidInLevel is Enemy) continue; //enemies don't collide with enemies
				if (this is Enemy && solidInLevel is Player) continue; //enemies don't collide with player
				if (this is Player && solidInLevel is Enemy) continue; //player doesn't collide with enemies
			}

			CollidingWithFloor = true;

			_vel.Add(Vector2.Mult(contactNormal,
				new Vector2(Mathf.Abs(_vel.x), Mathf.Abs(_vel.y))));
			switch (contactNormal.y)
			{
				case < -0.1f:
					CollidedWithFloor();
					break;
				case > 0.1f:
					CollidedWithCeiling();
					break;
			}

			if (Mathf.Abs(contactNormal.x) > 0.1f)
			{
				CollidedWithSide(contactNormal);
				if (solidInLevel is SolidClimbable)
				{
					_vel.y *= WALL_SLIDE_DRAG_MULTIPLIER;
				}
				else if (_vel.y < 0)
				{
					_vel.y *= PLATFORM_CLIMB_ASSIST_MULTIPLIER;
				}
			}

			if (_invincible)
			{
				float b = Mathf.Map(Time.time - _millisAtLastDamage, 0, MILLIS_BETWEEN_DAMAGES(), 0, 1); //TODO: Designer, make this blink
				SetColor(b, b, b);
			}
			else
			{
				SetColor(1, 1, 1);
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

	protected virtual void CollidedWithSide(Vector2 normal)
	{
	}

	protected virtual void CollidedWithFloor()
	{
		_vel.x *= FLOOR_WALK_DRAG_MULTIPLIER;
	}

	protected virtual void CollidedWithCeiling()
	{
	}

	public virtual bool TakeDamage(Vector2 directionOfAttack, int amount = 1)
	{
		if (_invincible) return false;
		_invincible = true;
		_millisAtLastDamage = Time.time;
		_health -= amount;
		if (_health <= 0)
			Die();
		ApplyForce(directionOfAttack);
		return true;
	}

	private void Die()
	{
		// Console.WriteLine(this + " oof");
		MyGame.LevelManager.CurrentLevel().RemoveEntity(this);
	}
}
