using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame;

public class Player : Entity
{
	//Variables for the designers:
	private const float PLAYER_MOVEMENT_SPEED = 1.6f;
	private const byte ANIMATION_DELAY = 100;
	private readonly Vector2 jump = new(0.0f, -60);

	public Player(Vector2 spawnPos) :
		base(spawnPos, "barry.png", 7, 1, 7, ANIMATION_DELAY)
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

		if(input.MagSq() > 0.1f)
			ApplyForce(input.Limit(1).Mult(PLAYER_MOVEMENT_SPEED));


		if (colliding && (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.SPACE)))
			ApplyForce(jump);

		base.Update();
	}
}
