using System.Collections.Generic;
using System.Linq;
using GXPEngine;
using MyGame.MyGame.Entities;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
	private readonly TiledLoader _tiledLoader;


	public Player Player;

	private readonly List<Solid> _solids = new();
	public int _totalWaves { private set; get; }
	public int _currentWave { private set; get; }
	public int MillisAtStart;

	public Level(string path)
	{
		_tiledLoader = new TiledLoader($"../../{path}", game);
		// Solids = new List<Solid>();
	}

	public void RemoveEntity(Entity e)
	{
		game.RemoveChild(e);
		_solids.Remove(e);
	}

	public void CreateLevel()
	{
		_tiledLoader.autoInstance = true;

		_tiledLoader.LoadImageLayers();
		_tiledLoader.LoadTileLayers();
		_tiledLoader.LoadObjectGroups();
		_totalWaves = 4; //TODO: Fill these two in with the actual right numbers
		_currentWave = 2;

		MillisAtStart = Time.time;
	}

	public List<Solid> GetVisibleSolids()
	{
		return _solids.Where(a => a.visible).ToList();
	}

	public List<Solid> GetAllSolids()
	{
		return _solids;
	}
	
	public void AddSolid(Solid s)
	{
		_solids.Add(s);
	}

	public void RemoveSolid(Solid s)
	{
		_solids.Remove(s);
	}

	public void InsertSolid(int i, Solid s)
	{
		_solids.Insert(i, s);
	}
}
