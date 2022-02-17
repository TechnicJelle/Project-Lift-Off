using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Walker : Enemy
{
	public Walker(TiledObject obj) : base(obj, 2)
	{
	}
}
