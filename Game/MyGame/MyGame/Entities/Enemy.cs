using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity
{
	private const byte ANIMATION_DELAY = 200;

	public Enemy(Vector2 spawnPos) :
		base(spawnPos, "evilBarry.png", 4, 2, 7, ANIMATION_DELAY, true)
	{
	}
	
	public Enemy(TiledObject obj) : base(new Vector2(0, 0), "evilBarry.png", 4, 2, 7, ANIMATION_DELAY, true)
	{
	}
}
