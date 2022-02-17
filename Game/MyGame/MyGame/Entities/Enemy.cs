using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private const byte ANIMATION_DELAY = 200;

	protected Enemy(TiledObject obj, int health) : base("evilBarry.png", 4, 2, 7, health, ANIMATION_DELAY, true)
	{
	}
}
