using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Solids;

public class Solid : Sprite
{
    public Solid(TiledObject obj) : base("barry.png")
    {
        this.width = (int)obj.Width;
        this.height = (int)obj.Height;
    }
    
    
}