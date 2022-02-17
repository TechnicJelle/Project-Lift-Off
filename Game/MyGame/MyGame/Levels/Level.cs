using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;

	public List<Solid> Solids = new();

	public int _totalWaves { private set; get; }
	public int _currentWave { private set; get; }

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
		_totalWaves = 4; //TODO: Fill these two in with the actual right numbers
		_currentWave = 2;
	}
}
