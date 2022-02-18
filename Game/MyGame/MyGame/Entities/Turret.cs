using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Turret : Enemy
{
	public Turret(TiledObject obj) : base("enemyShooter.png", 18, 3, 18, 1)
	{
		ApplyGravity = false;
	}
}
