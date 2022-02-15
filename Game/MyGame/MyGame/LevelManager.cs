using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GXPEngine;
using MyGame.MyGame.Levels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyGame.MyGame;

public class LevelManager
{
    public Dictionary<string, Level> Levels = new();
    private JObject _levels;
    private Game _game = Game.main;

    public LevelManager()
    {
        string text = File.ReadAllText("../../assets/maps/levels.json");

        try
        {
            _levels = JObject.Parse(text);

            foreach (var raw in _levels)
            {
                Levels.Add(raw.Key, new Level(raw.Value.ToString()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't load levels: {e}");
        }
    }

    public void Init()
    {
        _game.AddChild(GetMenu());
    }

    private Level GetMenu()
    {
        return Levels["menu"];
    }

    public Level GetLevel(string levelName)
    {
        return Levels[levelName];
    }

    public List<Level> GetLevels()
    {
        List<Level> temp = new();
        temp.AddRange(Levels.Values);

        return temp;
    }
}