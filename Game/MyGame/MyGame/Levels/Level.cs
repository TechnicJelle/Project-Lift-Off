using System.Collections.Generic;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;

	public readonly List<Solid> Solids;

	public int _totalWaves { private set; get; }
	public int _currentWave { private set; get; }

	public Level()
	{
		_tiledLoader = new TiledLoader("../../assets/maps/demo.tmx");
		CreateLevel();
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
		_totalWaves = 4; //TODO: Fill these two in with the actual right numbers
		_currentWave = 2;
	}
}
