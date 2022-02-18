using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame.Entities;

public class Bullet : Entity
{
	private const float MOVEMENT_SPEED = 1.5f;

	private readonly Vector2 _dir;

	public Bullet(float x, float y, Vector2 dir) : base("bullet.png", 1, 1, 1, 1)
	{
		this.x = x;
		this.y = y;

		ApplyGravity = false;

		_dir = dir;

		ApplyForce(Vector2.SetMag(dir, 1));

		rotation = Mathf.Degrees(dir.Heading());
	}


	protected new void Update()
	{
		if (!this.visible || MyGame.LevelManager.CurrentLevel().Player == null) return;
		base.Update();

		ApplyForce(Vector2.SetMag(_dir, MOVEMENT_SPEED));
	}
}
