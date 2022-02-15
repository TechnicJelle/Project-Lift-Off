using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
    private readonly TiledLoader _tiledLoader;

    public Level(string path)
    {
        _tiledLoader = new TiledLoader($"../../{path}");

        CreateLevel();
    }

    private void CreateLevel()
    {
        _tiledLoader.autoInstance = true;
        
        _tiledLoader.LoadImageLayers();
        _tiledLoader.LoadTileLayers();
        _tiledLoader.LoadObjectGroups();
    }
}