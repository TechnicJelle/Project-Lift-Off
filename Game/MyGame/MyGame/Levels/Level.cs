using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;

	public readonly List<Solid> Solids;

	public Level(string path)
	{
		_tiledLoader = new TiledLoader($"../../{path}");
		Solids = new List<Solid>();
		foreach (GameObject gameObject in game.GetChildren())
		{
			if (gameObject is Solid solid)
			{
				Solids.Add(solid);
			}
		}
	}

	private void CreateLevel()
	{
		_tiledLoader.autoInstance = true;

		_tiledLoader.LoadImageLayers();
		_tiledLoader.LoadTileLayers();
		_tiledLoader.LoadObjectGroups();
	}
}
