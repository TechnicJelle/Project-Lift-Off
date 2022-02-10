using System;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class Player : Entity
{
	//Variables for the designers:
	private const float PLAYER_MOVEMENT_SPEED = 1.6f;
	private const byte ANIMATION_DELAY = 100;
	private const float MIN_INSTANT_JUMP_FORCE = 30.0f; //when you jump, you always jump at least with this much force
	private const float MAX_GRADUAL_JUMP_FORCE = 5.0f; //every frame you're jumping, a fraction of this force is applied
	private const int MAX_JUMPS = 2;
	private const int MILLIS_FOR_MAX_JUMP = 500;


	//Variables needed to track the internal state of the player

	//Double Jump:
	private bool _inAir;
	private int _jumpAmounts;

	//Hold to jump higher:
	private bool _jumping;
	private int _millisAtStartJump;

	public Player(Vector2 spawnPos) :
		base(spawnPos, "playerIdle.jpg", 8, 2, 12, ANIMATION_DELAY)
	{
		//Empty
	}

	private new void Update()
	{




		Vector2 input = new(0.0f, 0.0f);

		if (Input.GetKey(Key.A))
		{
			input.x = -1;
		}

		if (Input.GetKey(Key.D))
		{
			input.x = 1;
		}

		if (input.MagSq() > 0.1f)
			ApplyForce(input.Limit(1).Mult(PLAYER_MOVEMENT_SPEED));

		if (_inAir && Colliding)
		{
			ResetJumps();
		}
		if (Colliding && _jumping || (Input.GetKeyUp(Key.W) || Input.GetKeyUp(Key.SPACE)))
		{
			StopJump();
		}
		if ((Colliding || _jumpAmounts < MAX_JUMPS) && (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.SPACE)))
		{
			StartJump();
		}

		if (_jumping)
		{
			int millisSinceJump = Time.time - _millisAtStartJump;
			//TODO: Make/Find a better formula for the jumpForce, preferably not a linear one like this, but instead an exponential one that starts off fast and then becomes less
			float jumpForce = Mathf.Map(
				Mathf.Clamp(MILLIS_FOR_MAX_JUMP - millisSinceJump, 0, MILLIS_FOR_MAX_JUMP),
				500, 0, MAX_GRADUAL_JUMP_FORCE, 0);
			ApplyForce(new Vector2(0, -jumpForce));
			if(MyGame.DEBUG_MODE) Console.WriteLine("jumping with force of " + jumpForce + ", " + millisSinceJump);
		}

		base.Update();
	}

	private void StartJump()
	{
		_millisAtStartJump = Time.time;
		_jumping = true;
		_inAir = true;
		_jumpAmounts++;
		ApplyForce(new Vector2(0, -MIN_INSTANT_JUMP_FORCE));
		Console.WriteLine("jump start");
	}

	private void StopJump()
	{
		_jumping = false;
		Console.WriteLine("jump end");
	}

	private void ResetJumps()
	{
		_inAir = false;
		_jumpAmounts = 0;
		Console.WriteLine("jump reset");
	}
}
