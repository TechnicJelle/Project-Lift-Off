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
    private Dictionary<string, Level> _levels = new();
    private Level _lastLevel;
    private readonly Game _game = Game.main;

    public LevelManager()
    {
        string text = File.ReadAllText("../../assets/maps/levels.json");

        try
        {
            var rawLevels = JObject.Parse(text);

            foreach (var raw in rawLevels)
            {
                _levels.Add(raw.Key, new Level(raw.Value.ToString()));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't load levels: {e}");
        }
    }

    public void Init()
    {
        SetLevel("test");
    }

    public void SetLevel(string levelName)
    {
        if (_lastLevel != null) _game.RemoveChild(_lastLevel);

        var level = _levels[levelName];
        level.CreateLevel();
        
        _game.AddChild(level);

        _lastLevel = _levels[levelName];
    }

    public void ClearLevels()
    {
        foreach (Level level in _levels.Values)
        {
            _game.RemoveChild(level);
        }
    }
}