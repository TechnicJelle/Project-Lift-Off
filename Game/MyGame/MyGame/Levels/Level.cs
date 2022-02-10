using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Levels;

public class Level : GameObject
{
    private readonly TiledLoader _tiledLoader;

    public Level()
    {
        _tiledLoader = new TiledLoader("../../assets/maps/demo.tmx");
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