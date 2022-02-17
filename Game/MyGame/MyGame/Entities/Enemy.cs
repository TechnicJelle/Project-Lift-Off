using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private const byte ANIMATION_DELAY = 200;

	public Enemy(TiledObject obj) : this(obj, 2)
	{
	}

	public Enemy(TiledObject obj, int health) : base("evilBarry.png", 4, 2, 7, health, ANIMATION_DELAY, true)
	{
	}
}
