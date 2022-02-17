using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;

	public List<Solid> Solids = new();

	public Level(string path)
	{
		_tiledLoader = new TiledLoader($"../../{path}", game);
		// Solids = new List<Solid>();
	}

	public void CreateLevel()
	{
		_tiledLoader.autoInstance = true;

		_tiledLoader.LoadImageLayers();
		_tiledLoader.LoadTileLayers();
		_tiledLoader.LoadObjectGroups();
	}
}
