using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Turret : Enemy
{
	public Turret(TiledObject obj) : base(obj, 2)
	{
	}
}
