using System;
using System.Collections.Generic;
using GXPEngine;
using MyGame.MyGame.Solids;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;

	public readonly List<Solid> Solids;

	public Level()
	{
		_tiledLoader = new TiledLoader("../../assets/maps/demo.tmx");
		CreateLevel();
		Solids = new List<Solid>();
		foreach (GameObject gameObject in game.GetChildren())
		{
			Console.WriteLine(gameObject + " : " + (gameObject is Solid));
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
