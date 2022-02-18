using System;
using System.Collections.Generic;
using System.Linq;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Player : Entity
{
	//Variables for the designers:
	//Movement:
	private const float PLAYER_MOVEMENT_SPEED = 100.0f;
	private const float DASH_THRESHOLD = 30.0f; //sorry ;-;

	//Attack:
	private const float ATTACK_REACH = 200.0f;
	private const float ATTACK_CONE = Mathf.HALF_PI;
	private const float ATTACK_FORCE_STRENGTH = 10.0f;

	//Animation:
	private const byte IDLE_ANIMATION_DELAY = 100;
	// private const byte RUN_MIN_ANIMATION_DELAY = 100;
	// private const byte RUN_MAX_ANIMATION_DELAY = 100;

	//Double Jump:
	private const int MAX_JUMPS = 2;

	//Hold to jump higher:
	private const int MILLIS_FOR_MAX_JUMP = 500;
	private const float MIN_INSTANT_JUMP_FORCE = 30.0f; //when you jump, you always jump at least with this much force
	private const float MAX_GRADUAL_JUMP_FORCE = 6.0f; //every frame you're jumping, a fraction of this force is applied
	private const float JUMP_FORCE_SLOPE = 0.5f;

	//Dash:
	private const float DASH_FORCE = 100.0f;
	private const int MILLIS_BETWEEN_DASHES = 4000;

	//Enemies:
	protected override int MILLIS_BETWEEN_DAMAGES() { return 2000; }


	//Variables needed to track the internal state of the player
	//Double Jump:
	private bool _inAir;
	private int _jumpAmounts;

	//Hold to jump higher:
	private bool _jumping;
	private int _millisAtStartJump;

	//Dash:
	private int _millisAtLastDash;
	private int _millisSinceLastDash;

	//Enemies:
	private List<Enemy> _currentlyCollidingWithEnemies;
	private bool _isAttacking;

	public Player(TiledObject obj) : base("player.png", 16, 5, 64, MyGame.PLAYER_HEALTH, IDLE_ANIMATION_DELAY)
	{
		//Empty
	}

	private new void Update()
	{
		GameObject[] cols = GetCollisions(true, true);
		_currentlyCollidingWithEnemies = new List<Enemy>();
		foreach (GameObject gameObject in cols)
		{
			if (gameObject is Enemy enemy && gameObject.visible)
				_currentlyCollidingWithEnemies.Add(enemy);
		}

		bool isDashing = _vel.MagSq() > DASH_THRESHOLD;
		// Console.WriteLine(this._vel.MagSq() > PLAYER_MOVEMENT_SPEED * PLAYER_MOVEMENT_SPEED * 1.1);
		// Console.WriteLine(_vel.MagSq());
		// SetAnimationDelay((byte) Mathf.Map(_vel.MagSq(), 0, 200, 255, 50));

		// Console.WriteLine("Dashing: " + isDashing);

		//Basic Left/Right Movement
		const float detail = 100.0f;
		// Console.WriteLine(Gamepad._joystick.x);
		float xMovement = Mathf.Clamp(Gamepad._joystick.x, -detail, detail) / detail;
		ApplyForce(Vector2.Mult(new Vector2(xMovement, 0), PLAYER_MOVEMENT_SPEED));


		if (xMovement != 0 && !_inAir && !isDashing && !_isAttacking)
		{
			this.SetCycle(49, 5);
		}
		else if (xMovement == 0 && !_inAir && !isDashing && !_isAttacking)
		{
			this.SetCycle(0, 12);
		}

		//Jumping Movement
		if (_inAir && CollidingWithFloor)
		{
			ResetJumps();
		}
		if (CollidingWithFloor && _jumping || (Input.GetKeyUp(Key.W) || Input.GetKeyUp(Key.SPACE)))
		{
			StopJump();
		}
		if ((CollidingWithFloor || _jumpAmounts < MAX_JUMPS) && (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.SPACE)))
		{
			StartJump();
		}

		if (Input.GetKeyDown(Key.Y))
		{
			if(MyGame.DEBUG_MODE) TakeDamage(new Vector2(-10, 0)); //TODO: actually call this form the right place
		}

		if (_jumping)
		{
			int millisSinceJump = Time.time - _millisAtStartJump;

			float jumpProgress = Mathf.Clamp(Mathf.Map(MILLIS_FOR_MAX_JUMP - millisSinceJump, 0, MILLIS_FOR_MAX_JUMP, 1.0f, 0.0f), 0.0f, 1.0f);


			//jumpProgress (range: 0.0f..1.0f) represents how long the jump has been going on for.
			//0.0f: when the jump has just started
			//1.0f: when the jump has reached its maximum height
			//DONE: Construct a better formula for the jumpForce, preferably not a linear one like this, but instead an exponential one
			// float jumpForce = Mathf.Map(jumpProgress, 0.0f, 1.0f, MAX_GRADUAL_JUMP_FORCE, 0.0f);
			float jumpForce = MAX_GRADUAL_JUMP_FORCE - MAX_GRADUAL_JUMP_FORCE * Mathf.Pow(jumpProgress, JUMP_FORCE_SLOPE);

			ApplyForce(new Vector2(0, -jumpForce));
			if(MyGame.DEBUG_MODE) Console.WriteLine("jumping with force of " + jumpForce + ". jump progress: " + jumpProgress);
		}

		if (Input.GetKeyDown(Key.E) || Input.GetMouseButtonDown(0))
		{
			this._isAttacking = true;
			this.SetCycle(65, 3, 255, true);
			Attack(_vel);

			// this._animationDelay = 1;
		}


		//Actually calculate and apply the forces that have been acting on the Player the past frame
		base.Update();

		if (_currentlyCollidingWithEnemies.Count > 0)
		{
			if (isDashing)
			{
				_millisAtLastDash = Time.time - MILLIS_BETWEEN_DASHES;

				foreach (Enemy enemy in _currentlyCollidingWithEnemies)
				{
					enemy.TakeDamage(Vector2.Mult(_vel, 2.0f));
				}
			}
			else
			{
				foreach (Enemy enemy in _currentlyCollidingWithEnemies)
				{
					TakeDamage(Vector2.Mult(enemy._vel, -2.0f));
					// Console.WriteLine(enemy.ToString());
				}
			}
		}

		//Dashing movement
		_millisSinceLastDash = Time.time - _millisAtLastDash;
		if (Input.GetKeyDown(Key.LEFT_SHIFT) || Input.GetMouseButtonDown(1))
		{
			RequestDash(Gamepad._joystick);
		}

		float dashCooldown = Mathf.Map(Mathf.Clamp(MILLIS_BETWEEN_DASHES - _millisSinceLastDash, 0, MILLIS_BETWEEN_DASHES), 0, MILLIS_BETWEEN_DASHES, 0, 1);

		UI.Canvas.Text("Dash Cooldown: " + dashCooldown); //TODO: Designer, make this into Arc
		this.Animate();
	}

	private void StartJump()
	{
		SoundManager.jump.Play();
		_millisAtStartJump = Time.time;
		_jumping = true;
		_inAir = true;
		_jumpAmounts++;
		this.SetCycle(33, 7);
		this.AnimateFixed();
		ApplyForce(new Vector2(0, -MIN_INSTANT_JUMP_FORCE));
		if (MyGame.DEBUG_MODE) Console.WriteLine("jump start");
	}

	private void StopJump()
	{
		_jumping = false;
		if (MyGame.DEBUG_MODE) Console.WriteLine("jump end");
	}

	private void ResetJumps()
	{
		_inAir = false;
		_jumpAmounts = 0;
		if (MyGame.DEBUG_MODE) Console.WriteLine("jump reset");
	}

	private void RequestDash(Vector2 direction)
	{
		if (MyGame.DEBUG_MODE) Console.WriteLine(_millisSinceLastDash);
		if (_millisSinceLastDash < MILLIS_BETWEEN_DASHES) return;
		Dash(direction);
	}

	private void Dash(Vector2 direction)
	{
		SoundManager.dash.Play(volume: 0.6f);
		_millisAtLastDash = Time.time;
		this.SetCycle(17, 5);
		this.AnimateFixed();
		ApplyForce(Vector2.Mult(direction.Copy().Normalize(), DASH_FORCE));
		MyGame.AddScore(10);
	}

	public override bool TakeDamage(Vector2 directionOfAttack, int amount = 1)
	{
		if (!base.TakeDamage(directionOfAttack, amount)) return false;
		UI.ReduceHearts(amount);
		return true;
	}

	private void Attack(Vector2 direction)
	{
		SoundManager.weaponSwoosh.Play();
		Vector2 playerPos = new(x, y);
		foreach (Enemy e in MyGame.LevelManager.CurrentLevel().GetVisibleSolids().OfType<Enemy>())
		{
			Vector2 enemyPos = new(e.x, e.y);

			if (Vector2.Dist(playerPos, enemyPos) > ATTACK_REACH) continue; //Out of reach

			Vector2 toTarget = Vector2.Sub(enemyPos, playerPos);
			float angleBetween = Vector2.AngleBetween(direction, toTarget);
			if (angleBetween > ATTACK_CONE) continue; //Not in angle range

			e.TakeDamage(_vel.Copy().SetMag(ATTACK_FORCE_STRENGTH));
		}

		this._isAttacking = false;
	}

	protected override void CollidedWithCeiling()
	{
		StopJump();
		CollidingWithFloor = false;
		base.CollidedWithCeiling();
	}
}
