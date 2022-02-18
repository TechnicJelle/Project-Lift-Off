using System;
using GXPEngine;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Enemy : Entity {

	protected Enemy(string filename, int cols, int rows, int frames, int health) : base(filename, cols, rows, frames, health)
	{
	}
}
