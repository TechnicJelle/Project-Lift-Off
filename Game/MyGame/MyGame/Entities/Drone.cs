using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Drone : Enemy
{
	private const float MOVEMENT_SPEED = 0.5f;

	public Drone(TiledObject obj) : base("enemyDrone.png", 12, 2, 11, 1)
	{
		ApplyGravity = false;
	}

	protected new void Update()
	{
		Vector2 toTarget = Vector2.Sub(MyGame.LevelManager.CurrentLevel().Player.GetPos(), new Vector2(x, y));
		ApplyForce(toTarget.SetMag(MOVEMENT_SPEED));
		base.Update();
	}
}
